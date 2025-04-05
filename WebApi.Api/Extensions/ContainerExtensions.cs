using System.Data;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Api.Core;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.AppSettings;
using WebApi.Application.Jwt;
using WebApi.Application.Localization;
using WebApi.Application.Logging;
using WebApi.Application.Search;
using WebApi.Application.UseCases;
using WebApi.Application.Validation;
using WebApi.Common.Enums.Auth;
using WebApi.DataAccess;
using WebApi.Implementation.ApplicationUsers;
using WebApi.Implementation.Core;
using WebApi.Implementation.Extensions;
using WebApi.Implementation.Jwt;
using WebApi.Implementation.Localization;
using WebApi.Implementation.Logging;
using WebApi.Implementation.Search;
using WebApi.Implementation.UseCases;
using WebApi.Implementation.Validators;

namespace WebApi.Api.Extensions
{
    public static class ContainerExtensions
    {
        public static void SetupApplication(this WebApplicationBuilder builder)
        {
            var appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);

            builder.Services.AddSwagger();
            builder.Services.RegisterDependencies(appSettings);
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
            });
        }

        private static void RegisterDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton(appSettings);
            services.AddSingleton(appSettings.JwtSettings);

            services.AddTransient<ILocaleGetter, LocaleGetter>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext(appSettings);
            services.AddJwt(appSettings);
            services.AddApplicationUser();
            services.AddUseCases();
            services.AddAutoMapper();
            services.SetupLocalization();

            services.AddTransient(provider => new SqlConnection(appSettings.ConnectionStrings.Primary));
            services.AddTransient<IDbConnection>(provider => provider.GetService<SqlConnection>());

            services.AddTransient<EntityAccessor>();
            services.AddTransient<IUseCaseLogger, ConsoleUseCaseLogger>();
            services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();
            services.AddTransient<TokenCryptor>();
            services.AddTransient<ClaimsGenerator>();
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
            services.AddTransient<IJwtTokenStorage, EfJwtTokenStorage>();
            services.AddTransient<ISearchObjectQueryBuilder, EfSearchObjectQueryBuilder>();
        }

        private static void AddDbContext(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient(x =>
            {
                var options = new DbContextOptionsBuilder<DatabaseContext>()
                                    .EnableSensitiveDataLogging()
                                    .UseLazyLoadingProxies()
                                    .UseSqlServer(appSettings.ConnectionStrings.Primary, x => x.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name))
                                    .UseLazyLoadingProxies()
                                    .Options;

                var context = new DatabaseContext(options);

                return context;
            });

            services.AddTransient<DbContext>(x => x.GetService<DatabaseContext>());

            var config = Configuration.GetConfiguration<AppSettings>();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(config.ConnectionStrings.Primary, x => x.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name))
                       .UseLazyLoadingProxies();
            });
        }

        private static void AddJwt(this IServiceCollection services, AppSettings appSettings)
        {
            var jwtSettings = appSettings.JwtSettings;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                    {
                        if (expires.HasValue)
                        {
                            return DateTime.UtcNow < expires.Value;
                        }

                        return false;
                    }
                };
            });

            services.AddAuthorization();
        }

        private static void AddApplicationUser(this IServiceCollection services)
        {
            services.AddTransient<IApplicationUser>(provider =>
            {
                var localeGetter = provider.GetService<ILocaleGetter>();
                var locale = localeGetter.GetLocale();

                var accessor = provider.GetService<IHttpContextAccessor>();
                var userRoleUseCaseMap = provider.GetService<UserRoleUseCaseMapStore>();

                var anonymousUser = new AnonymousUser
                {
                    Locale = locale,
                    AllowedUseCases = userRoleUseCaseMap.GetUseCases(UserRole.Anonymous)
                };

                if (accessor?.HttpContext?.User is null)
                {
                    return anonymousUser;
                }

                var claims = accessor.HttpContext.User;

                if (claims.FindFirst(WebApiClaims.UserId) is null)
                {
                    return anonymousUser;
                }

                var userRole = Enum.Parse<UserRole>(claims.FindFirst(WebApiClaims.Role).Value);

                if (userRole == UserRole.Anonymous)
                {
                    return anonymousUser;
                }

                return new ApplicationUser
                {
                    Id = int.Parse(claims.FindFirst(WebApiClaims.UserId).Value),
                    Email = claims.FindFirst(WebApiClaims.Email).Value,
                    Role = userRole,
                    Locale = locale,
                    AllowedUseCases = userRoleUseCaseMap.GetUseCases(userRole)
                };
            });
        }

        #region UseCases
        private static void AddUseCases(this IServiceCollection services)
        {
            services.AddTransient<UseCaseMediator>();

            services.AddUseCaseHandlers();
            services.AddUseCaseValidators();
            services.AddUseCaseSubscribers();
            services.AddUserRoleUseCaseMapStore();
        }

        private static void AddUseCaseValidators(this IServiceCollection services)
        {
            services.AddImplementationsByBaseType<IValidator>([typeof(UserRoleUseCaseMapStore).Assembly]);
            services.AddTransient<IValidatorResolver, ServiceProviderValidatorResolver>();
        }

        private static void AddUseCaseHandlers(this IServiceCollection services)
        {
            services.AddImplementationsByBaseType<IUseCaseHandlerBase>([typeof(UserRoleUseCaseMapStore).Assembly]);
            services.AddTransient<IUseCaseHandlerResolver, ServiceProviderUseCaseHandlerResolver>();
        }

        private static void AddUseCaseSubscribers(this IServiceCollection services)
        {
            var interfaceType = typeof(IUseCaseSubscriber<,,>);
            var assembliesToLookThrough = new Assembly[] { typeof(UserRoleUseCaseMapStore).Assembly };

            var useCaseSubscribersTypesData = interfaceType.GetGenericInterfaceImplementationTypes(assembliesToLookThrough);

            var groupedUseCaseSubscribersTypesData = useCaseSubscribersTypesData.GroupBy(x => x.ImplementedInterface).Select(x => x.AsEnumerable());

            foreach (var typesGroup in groupedUseCaseSubscribersTypesData)
            {
                foreach (var typeData in typesGroup)
                {
                    services.AddTransient(typeData.ImplementedInterface, typeData.ImplementationType);
                }
            }

            services.AddTransient<IUseCaseSubscriberResolver, ServiceProviderUseCaseSubscriberResolver>();
        }

        private static void AddUserRoleUseCaseMapStore(this IServiceCollection services)
        {
            var useCasesMap = new Dictionary<UserRole, List<string>>();

            foreach (var role in Enum.GetValues<UserRole>())
            {
                useCasesMap.Add(role, new List<string>());
            }

            var useCaseTypes = typeof(UseCase<,>).Assembly.GetImplementationsOfGenericType(typeof(IUseCase<,>));

            foreach (var useCaseType in useCaseTypes)
            {
                var allowForRolesAttribute = useCaseType.GetAttributeOfType<AllowForRolesAttribute>();

                if (allowForRolesAttribute is null)
                {
                    continue;
                }

                var constructorParameters = useCaseType.GetConstructors().OrderBy(x => x.GetParameters().Count()).FirstOrDefault()?.GetParameters();
                var constructorParametersDefaultValues = constructorParameters?.Select(x => x.GetType().GetDefault()).ToArray();

                var useCaseInstance = (IUseCaseBase?)Activator.CreateInstance(useCaseType, constructorParametersDefaultValues);

                if (useCaseInstance is null)
                {
                    continue;
                }

                foreach (var allowedRole in allowForRolesAttribute.Roles)
                {
                    useCasesMap[allowedRole].Add(useCaseInstance.Id);
                }
            }

            var store = new UserRoleUseCaseMapStore(useCasesMap);

            services.AddSingleton(store);
        }
        #endregion

        private static void AddImplementationsByBaseType<T>(this IServiceCollection services, Assembly[] assemblies)
        {
            var type = typeof(T);
            var types = assemblies.SelectMany(s => s.GetTypes())
                                  .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && !p.Name.Contains("Generic"));

            foreach (var t in types)
            {
                var baseType = t.BaseType;

                if (baseType != null)
                {
                    while (baseType.BaseType != null && baseType.BaseType.FullName != "System.Object")
                    {
                        baseType = baseType.BaseType;
                    }

                    services.AddTransient(baseType, t);
                }
            }
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void SetupLocalization(this IServiceCollection services)
        {
            services.AddTransient<ITranslator, SqlTranslator>();
        }
    }
}

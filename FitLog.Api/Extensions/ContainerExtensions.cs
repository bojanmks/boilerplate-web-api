using FitLog.Api.Core;
using FitLog.Api.ExceptionHandling;
using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Application.ApplicationUsers;
using FitLog.Application.AppSettings;
using FitLog.Application.Jwt;
using FitLog.Application.Localization;
using FitLog.Application.Logging;
using FitLog.Application.UseCases;
using FitLog.Common.Enums;
using FitLog.DataAccess;
using FitLog.Implementation.ApplicationUsers;
using FitLog.Implementation.Jwt;
using FitLog.Implementation.Localization;
using FitLog.Implementation.Logging;
using FitLog.Implementation.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace FitLog.Api.Extensions
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FitLog.Api", Version = "v1" });
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
            services.AddExceptionResponseGenerators();
            services.SetupLocalization(appSettings);

            services.AddTransient(provider => new SqlConnection(appSettings.ConnectionStrings.Primary));
            services.AddTransient<IDbConnection>(provider => provider.GetService<SqlConnection>());

            services.AddTransient<UserRoleUseCaseMap>();
            services.AddTransient<IUseCaseLogger, ConsoleUseCaseLogger>();
            services.AddTransient<IExceptionLogger, ConsoleExceptionLogger>();
            services.AddTransient<IExceptionResponseGeneratorGetter, ExceptionResponseGeneratorGetter>();
            services.AddTransient<TokenCryptor>();
            services.AddTransient<ClaimsGenerator>();
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
            services.AddTransient<IJwtTokenStorage, JwtTokenStorage>();
        }

        private static void AddDbContext(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient(x =>
            {
                var options = new DbContextOptionsBuilder<FitLogContext>()
                                    .EnableSensitiveDataLogging()
                                    .UseLazyLoadingProxies()
                                    .UseSqlServer(appSettings.ConnectionStrings.Primary, x => x.MigrationsAssembly("FitLog.Api"))
                                    .UseLazyLoadingProxies()
                                    .Options;

                var context = new FitLogContext(options);

                return context;
            });

            var config = Configuration.GetConfiguration<AppSettings>();

            services.AddDbContext<FitLogContext>(x =>
            {
                x.UseSqlServer(config.ConnectionStrings.Primary, x => x.MigrationsAssembly("FitLog.Api"))
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
                var userRoleUseCaseMap = provider.GetService<UserRoleUseCaseMap>();

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

                if (claims.FindFirst(FitLogClaims.UserId) is null)
                {
                    return anonymousUser;
                }

                var userRole = Enum.Parse<UserRole>(claims.FindFirst(FitLogClaims.Role).Value);

                if (userRole == UserRole.Anonymous)
                {
                    return anonymousUser;
                }

                return new ApplicationUser
                {
                    Id = int.Parse(claims.FindFirst(FitLogClaims.UserId).Value),
                    Email = claims.FindFirst(FitLogClaims.Email).Value,
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
        }

        private static void AddUseCaseValidators(this IServiceCollection services)
        {
            services.AddImplementationsByBaseType<IValidator>(new Assembly[] { typeof(UserRoleUseCaseMap).Assembly });
        }

        private static void AddUseCaseHandlers(this IServiceCollection services)
        {
            services.AddImplementationsByBaseType<IUseCaseHandler>(new Assembly[] { typeof(UserRoleUseCaseMap).Assembly });
        }

        private static void AddUseCaseSubscribers(this IServiceCollection services)
        {
            var interfaceType = typeof(IUseCaseSubscriber<,,>);
            var assembliesToLookThrough = new Assembly[] { typeof(UserRoleUseCaseMap).Assembly };

            var useCaseSubscribersTypesData = interfaceType.GetGenericInterfaceImplementationTypes(assembliesToLookThrough);

            var groupedUseCaseSubscribersTypesData = useCaseSubscribersTypesData.GroupBy(x => x.ImplementedInterface).Select(x => x.AsEnumerable());

            foreach (var typesGroup in groupedUseCaseSubscribersTypesData)
            {
                foreach (var typeData in typesGroup)
                {
                    services.AddTransient(typeData.ImplementedInterface, typeData.ImplementationType);
                }
            }
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

        private static void AddExceptionResponseGenerators(this IServiceCollection services)
        {
            var interfaceType = typeof(IExceptionResponseGenerator<>);
            var assembliesToLookThrough = new Assembly[] { Assembly.GetExecutingAssembly() };

            var exceptionResponseGeneratorTypesData = interfaceType.GetGenericInterfaceImplementationTypes(assembliesToLookThrough);
            
            foreach (var typeData in exceptionResponseGeneratorTypesData)
            {
                services.AddTransient(typeData.ImplementedInterface, typeData.ImplementationType);
            }
        }

        private static void SetupLocalization(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<ITranslator, SqlTranslator>();
        }
    }
}

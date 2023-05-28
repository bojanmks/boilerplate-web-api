using FitLog.Api.Extensions;
using FitLog.Api.Middleware;
using FitLog.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.SetupApplication();

var app = builder.Build();

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyMethod();
    x.AllowAnyHeader();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ServiceProviderGetter.SetupProvider(app.Services);

app.Run();

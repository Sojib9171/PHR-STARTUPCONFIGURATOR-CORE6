using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using HBS.ImplementationAutomationConsole.API.Security;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using LiteDAC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, lc) => lc
         .MinimumLevel.Debug()
         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
         .Enrich.FromLogContext()
         .ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddControllers();

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddMemoryCache();



        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.Path = "/";
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
        {
            builder.WithOrigins("http://localhost.ia_ui.com:8010").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("http://localhost:8010").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("http://52.184.83.3:8010").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("http://20.187.179.166:8010").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("https://phrconfigassist.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            builder.WithOrigins("https://acod.phrconfig.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("User", policy => policy.RequireRole("User"));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User Admin", policy => policy.RequireRole("User Admin"));
        });

        builder.Services.AddLiteDbService();

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    new string[] { }
                }
            });
        });

        builder.Services.InjectDependencies();

        var app = builder.Build();

        if (app.Environment.IsDevelopment() /*|| app.Environment.IsProduction()*/)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //security headers
        //app.Use(async (context, next) =>
        //{
        //    //'unsafe-inline'
        //    context.Response.Headers.Add("Content-Security-Policy",
        //        "script-src 'self' 'unsafe-inline' ; frame-ancestors 'none'");
        //    context.Response.Headers.Add("X-Frame-Options", "DENY");
        //    context.Response.Headers.Remove("X-Powered-By");
        //    context.Response.Headers.Remove("Server");
        //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        //    await next();
        //});

        //app.UseHsts();

        app.UseHttpsRedirection();

        app.UseCors("corsapp");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSession();

        app.MapControllers();

        app.Run();
    }

}
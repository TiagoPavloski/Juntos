using Juntos.Teste.ApplicationService.Services;
using Juntos.Teste.Domain.ContextJuntos;
using Juntos.Teste.Domain.Contracts.Repository;
using Juntos.Teste.Domain.Contracts.Services;
using Juntos.Teste.Domain.Models;
using Juntos.Teste.Domain.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Juntos.Teste
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDbContext<JuntosContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ConnPrincipal")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDapperRepository, DapperRepository>();

            services.AddScoped<IAuthenticateService, AuthenticationService>();
            services.AddScoped<IUsuarioService, UsuarioService>();



            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Juntos Seguros",
                        Version = "v3.0",
                        Description = "Exemplo de API REST criada com o ASP.NET Core 3.0 para cadastro de usuários",
                        Contact = new OpenApiContact
                        {
                            Name = "Tiago Pavloski",
                            Url = new Uri("https://github.com/TiagoPavloski")
                        }
                    });

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Entre com o token JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                c.AddSecurityDefinition(
                    "jwt_auth", securityDefinition);

                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                c.AddSecurityRequirement(securityRequirements);
            });

            services.Configure<ConnString>(Configuration.GetSection("ConnectionStrings"));

            //var jwtSettings = new TokenManagement();
            //new ConfigureFromConfigurationOptions<TokenManagement>(Configuration.GetSection("tokenManagement"))
            //    .Configure(jwtSettings);
            //services.AddSingleton(jwtSettings);


            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));

            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var secret = token.Secret;


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = false,
                     ValidateIssuerSigningKey = false,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                     ClockSkew = TimeSpan.Zero
                 });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", policy => policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser());

                //auth.AddPolicy("Permission", policy => policy
                //    .RequireAuthenticatedUser());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
        }
    }
}

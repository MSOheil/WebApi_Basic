using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiBasic.Data;
using WebApiBasic.Repositories;
using WebApiBasic.Servises;
using Serilog;
using WebApiBasic.MiddleWare;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using WebApiBasic.Security;

namespace WebApiBasic
{
    public class Startup
    {
        //readonly string MyAllowSpecificOrigins = "http://localhost:4200";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiBasic", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            #region DB
            services.AddDbContextPool<DBContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("ConnectionToSql"))
            );
            #endregion
            #region Cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:4200")
            //                                      .AllowAnyHeader()
            //                                      .AllowAnyMethod();
            //                      });
            //});
            #endregion
            #region  AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion
            #region dependency
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IPatientService, PatientService>();
            #endregion
            #region IdentityConfigure
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DBContext>()
                .AddDefaultTokenProviders();
            #endregion
            #region External Provider
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddGoogle(options =>
                {
                    options.ClientId = "477522169839-e64o1p68ji0h3pntkftmj4bgcph0v8bg.apps.googleusercontent.com";
                    options.ClientSecret = "Q_P1c-7LyLe_6gKNPKbIooSJ";
                });
            #endregion
            #region JWTBearer
            var jwtSettings = Configuration.GetSection("JWTSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtbeareOptions =>
            {
                jwtbeareOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            }
           );
            #endregion
            #region Add Policy Authentication
            services.AddAuthorization(sd =>
            sd.AddPolicy("RolePolicy",
                   policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()))
            );
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiBasic v1"));
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            //app.UseCors(MyAllowSpecificOrigins);


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

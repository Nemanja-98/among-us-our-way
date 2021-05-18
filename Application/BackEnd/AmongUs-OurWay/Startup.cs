using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Hubs;
using Newtonsoft.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AmongUs_OurWay.DataManagement;
using Microsoft.AspNetCore.Http;

namespace AmongUs_OurWay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AmongUsContext>();
            services.AddControllers().AddNewtonsoftJson(options =>{
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddXmlSerializerFormatters();

            services.AddSignalR(options =>{
                options.EnableDetailedErrors = true;
            });
            // .AddJsonProtocol(options => {
            //     options.PayloadSerializerOptions.WriteIndented = true;
            // }).AddMessagePackProtocol();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmongUs_OurWay", Version = "v1" });
            });

            services.AddCors(options =>{
                options.AddPolicy("ServerPolicyV1", options=>{
                    options.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://192.168.1.199:4200")
                    .AllowCredentials();
                });
            });

            services.AddSingleton<LiveUsersMenager>(new LiveUsersMenager());
            
            services.AddSingleton<LiveGamesMenager>(new LiveGamesMenager());

            services.AddSingleton<Repository>(new Repository(new RepositoryConnection()));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
                options.TokenValidationParameters=new TokenValidationParameters{
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AmongUsAwesomeSeacretKey"))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmongUs_OurWay v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ServerPolicyV1");

            app.UseWebSockets();

            app.Use(async (context, next) => await AuthQueryStringToHeader(context, next));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<FriendHub>("/friend");
            });
        }

        private async Task AuthQueryStringToHeader(HttpContext context, Func<Task> next)
        {
            var queryString = context.Request.QueryString;

            if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]) && queryString.HasValue)
            {
                var token = (from pair in queryString.Value.TrimStart('?').Split('&')
                        where pair.StartsWith("access_token=")
                        select pair.Substring(13)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }

            await next?.Invoke();
        }
    }
}

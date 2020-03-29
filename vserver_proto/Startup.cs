using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using vserver_proto.Data;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Microsoft.AspNetCore.Components.Authorization;
using vserver_proto.Repositories.Authentication;
using System.Net.Http;

namespace vserver_proto
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true);
            // string value = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.GetApplicationDefault(),
            //    ServiceAccountId = "firebase-adminsdk-w2m8q@vserve-app.iam.gserviceaccount.com",
            //});
            FileStream serviceAccount = new FileStream(@"vserve-app-firebase-adminsdk-w2m8q-0e8b9f15f9.json",FileMode.Open,FileAccess.Read,FileShare.Read);
            FirebaseAdmin.AppOptions options = new FirebaseAdmin.AppOptions
            {
                Credential = GoogleCredential.FromStream(serviceAccount),
                ServiceAccountId = "firebase-adminsdk-w2m8q@vserve-app.iam.gserviceaccount.com",
            };

            FirebaseAdmin.FirebaseApp.Create(options);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Authority = "https://securetoken.google.com/vserve-app";
                    options.Audience = "https://securetoken.google.com/vserve-app";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/vserve-app",
                        ValidateAudience = true,
                        ValidAudience = "vserve-app",
                        ValidateLifetime = true
                    };
                });

            services.AddOptions();
            services.AddAuthenticationCore(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
            services.AddScoped<AuthenticationStateRepo>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthenticationStateRepo>());
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<LoginService>();
            services.AddSingleton<RegistrationService>();
            services.AddScoped<IFireBaseRepo, FireBaseRepo>();
            services.AddScoped<DataService>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

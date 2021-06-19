using FinanceChat.Data;
using FinanceChat.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace FinanceChat
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            // Set RequiredConfirmedAccount = false to skip confirmation mail
            services.AddDefaultIdentity<ChatUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddTransient<HttpClient>();
            services.AddScoped<Back.IDataAccess, Back.DataAccess>();
            services.AddTransient<Services.IStockService, Services.StockService>();
            services.AddTransient<Services.IMessageProcessorService, Services.MessageProcessorService>();
            services.AddRazorPages();
            // Add SignalR Service - Service to send / receive messages
            services.AddSignalR(hubOptions =>
            {
                hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(3);
                hubOptions.ClientTimeoutInterval = System.TimeSpan.FromMinutes(6);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
                endpoints.MapRazorPages();
                // Map Hub with custom
                endpoints.MapHub<Hubs.Chat>("/chat");
            });
        }
    }
}

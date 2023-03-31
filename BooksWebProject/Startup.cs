using BooksWebProject.Permission;
using Domin.Entity;
using Infarstructure.IRepository;
using Infarstructure.IRepository.ServicesRepository;
using Infarstuructre;
using Infarstuructre.Data;
//using Infarstuructre.IRepository;
//using Infarstuructre.IRepository.ServicesRepository;
using Infarstuructre.ViewModel;
using infrastructure.IRepository.ServicesRepository;
using Infrastructure.IRepository.ServicesRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBook
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
            services.AddControllersWithViews();
            //services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddSession();
            services.AddDbContext<FreeBookDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BookConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<FreeBookDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin";
                options.AccessDeniedPath = "/Admin/Home/Denied";
            });

            //

            services.AddScoped<IServicesRepository<Category>, ServicesCategory>();



            services.AddScoped<IServesisRepositoryLog<LogCategory>, ServicesLogCategory>();
            services.AddScoped<IServicesRepository<SubCategory>, ServicesSubCategory>();
            services.AddScoped<IServesisRepositoryLog<LogSubCategory>, ServicesLogSubCategory>();
            services.AddScoped<IServicesRepository<Book>, ServicesBook>();
            services.AddScoped<IServesisRepositoryLog<LogBook>, ServicesLogBook>();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();//AddSingleton عشان يظلو متصل يعني الاتصال مفتوح
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();//AddScoped هاي بس لما تبعث ركوست


            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = false;  
            //    options.Password.RequireLowercase = false;  
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredUniqueChars = 0;
            //    options.Password.RequiredLength = 5;
            //    options.Password.RequireNonAlphanumeric = false;
            //});

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Accounts}/{action=Login}/{id?}"
             );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using System;
using DidSayIt.Data;
using DidSayIt.Repository;
using DidSayIt.Services.ContextServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DidSayIt.Services.IdentityMessaging;
using DidSayItModels;
using DidSayItModels.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DidSayIt
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Environment.GetEnvironmentVariable("connectionstring") ?? throw new Exception("connectionstring cannot be null")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(4);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = "/Home/Login";
                    options.AccessDeniedPath = "/Home/AccessDenied";
                    options.SlidingExpiration = true;
                });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddHttpContextAccessor();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<IdentityRole>, RoleManager<IdentityRole>>();

            //Repositories
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<ISubdomainRepository, SubdomainRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            AppContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            RunMigrations(app);
            SeedData(serviceProvider);
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public bool SeedData(IServiceProvider serviceProvider)
        {
            try
            {
                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                DbSeeder.SeedData(userManager, roleManager).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running migrations. {e.Message}");
            }

            return true;
        }


        public bool RunMigrations(IApplicationBuilder app)
        {
            try
            {
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    db.Database.EnsureCreated();
                    db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running migrations. {e.Message}");
            }

            return true;
        }
    }
}

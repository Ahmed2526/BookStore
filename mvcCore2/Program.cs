using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Repository.IRepository;
using ModelsLayer.Models;
using DAL.Repository.Repository;
using Microsoft.AspNetCore.Identity;

namespace WebLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt => 
            { 
                opt.User.RequireUniqueEmail = true; 
                opt.SignIn.RequireConfirmedAccount = true; 
            })
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

           
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAuthentication()
              .AddGoogle(opt =>
              {
                  opt.ClientId = "231862772308-jsgviuoslpgu0llddch4mrhjnltggp84.apps.googleusercontent.com";
                  opt.ClientSecret = "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";
              })
              .AddFacebook(opt =>
              {
                  opt.AppId = "1386807352060439";
                  opt.AppSecret = "51252cea15c70191ae8f4ee0589ae5ad";
              });


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();;

            app.UseAuthorization();

            app.MapRazorPages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
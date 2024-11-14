using AutoMapper;
using Blog.BLL.Interfaces;
using Blog.BLL.Repositories;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Blog.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blog.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(option => 
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
                
            // Repo Implementation & Interface
            builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();
            builder.Services.AddScoped<IRepositoryComment, RepositoryComment>();
            builder.Services.AddScoped<IRepositoryCommentLike, RepositoryCommentLike>();
            builder.Services.AddScoped<IRepositoryCommentReport, RepositoryCommentReport>();
            builder.Services.AddScoped<IRepositoryFollow, RepositoryFollow>();
            builder.Services.AddScoped<IRepositoryPost, RepositoryPost>();
            builder.Services.AddScoped<IRepositoryPostLike, RepositoryPostLike>();
            builder.Services.AddScoped<IRepositoryPostReport, RepositoryPostReport>();
            builder.Services.AddScoped<IRepositoryUserReport, RepositoryUserReport>();
            //Mapper
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

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

            app.UseAuthorization();
            // Area
            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

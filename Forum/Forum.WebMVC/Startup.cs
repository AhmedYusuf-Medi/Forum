//Local
using Forum.Data;
using Forum.Data.ModelBuilderExtension.Seeder;
using Forum.Service;
using Forum.Service.Contracts;
//Static
using static Forum.Service.Common.Message.Message;
//Nuget packets
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Forum.WebMVC
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
            services.AddDbContext<ForumDbContext>(options =>
            options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostLikeService, PostLikeService>();
            services.AddScoped<ICommentLikeService, CommentLikeService>();
            services.AddScoped<ISelectListService, SelectListService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMailService, MailService>();

            //Cloudinary service
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Cloudinary Setup
            Cloudinary cloudinary = new Cloudinary(new Account
                (
                   ExternalProvider.Cloudinary_Username,
                   ExternalProvider.Cloudinary_ApiKey,
                   ExternalProvider.Cloudinary_ApiSecret
                ));
            services.AddSingleton(cloudinary);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ForumDbContext>();

                    if (!env.IsDevelopment())
                    {
                        dbContext.Database.Migrate();
                    }

                    new ForumDbContextSeeder()
                        .SeedAsync(dbContext)
                        .GetAwaiter()
                        .GetResult();
                }
            }

            app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
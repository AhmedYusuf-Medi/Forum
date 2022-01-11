//Local
using Forum.Data;
using Forum.Data.ModelBuilderExtension.Seeder;
using Forum.Service;
using Forum.Service.Contracts;
using Forum.WebApi.Helpers;
//Nuget packets
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
//Static
using static Forum.Service.Common.Message.Message;
//Public
using System;
using System.IO;
using System.Reflection;

namespace Forum.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ForumDbContext>(options =>
            options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Forum" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPostLikeService, PostLikeService>();
            services.AddScoped<ICommentLikeService, CommentLikeService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPostReportService, PostReportService>();
            services.AddScoped<ICommentReportService, CommentReportService>();
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

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum"));
            }

            app.UseExceptionHandler(ExceptionHandler.HandleExceptions());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
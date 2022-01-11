//Local
using Forum.Data;
using Forum.Service;
using Forum.Service.Contracts;
using Forum.Test.Services.Storage;
//Nuget packets
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CloudinaryDotNet;
//Public
using System.Threading.Tasks;

namespace Forum.Test.Services
{
    public abstract class BaseTest
    {
        protected DbContextOptions<ForumDbContext> Options { get; set; }

        protected ICloudinaryService CloudinaryService { get; set; }

        protected  IMailService MailService { get; set; }

        private SqliteConnection connection = new SqliteConnection("DataSource=:memory:");

        [TestInitialize]
        public async Task Initialize()
        {
            var mockedCloudinary = new Mock<Cloudinary>();
            var mockCloudinaryService = new Mock<CloudinaryService>(mockedCloudinary);
            this.CloudinaryService = mockCloudinaryService as ICloudinaryService;

            var mockMailService = new Mock<MailService>();
            this.MailService = mockMailService.Object;

            this.connection.Open();

            this.Options = new DbContextOptionsBuilder<ForumDbContext>().UseSqlite(connection).Options;

            using (var dbContext = new ForumDbContext(Options))
            {
                dbContext.Database.EnsureCreated();

                await Seeder.SeedAsync(dbContext);
                //Seeder.Seed(dbContext);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
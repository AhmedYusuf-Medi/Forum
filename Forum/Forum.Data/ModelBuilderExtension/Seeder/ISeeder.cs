using System.Threading.Tasks;

namespace Forum.Data.ModelBuilderExtension.Seeder
{
    public interface ISeeder
    {
        Task SeedAsync(ForumDbContext dbContext);
    }
}
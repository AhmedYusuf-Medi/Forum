//Local
using Forum.Data;
using Forum.Models.Entities;
using Forum.Service.Contracts;
//Nuget packets
using Microsoft.EntityFrameworkCore;
//Public
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Service
{
    public class SelectListService : ISelectListService
    {
        private readonly ForumDbContext db;

        public SelectListService(ForumDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await this.db.Categories
                    .Select
                    (
                       c => new Category
                       {
                           Id = c.Id,
                           Name = c.Name
                       }
                    ).ToListAsync();
        }
    }
}

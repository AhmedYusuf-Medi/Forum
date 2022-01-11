using Forum.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface ISelectListService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
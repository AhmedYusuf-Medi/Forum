using Forum.Models.Response;
using System;
using System.Threading.Tasks;

namespace Forum.Service.Contracts
{
    public interface IMailService
    {
        Task<InfoResponse> SendMailAsync(string email, Guid code);
    }
}
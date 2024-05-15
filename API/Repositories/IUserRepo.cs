using API.Models.DB;
using API.Models.DTO;

namespace API.Repositories
{
    public interface IUserRepo : IRepoBase<User>
    {
        Task<User?> GetByUidAsync(string uid);
       
    }
}

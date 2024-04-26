using API.Models.DB;

namespace API.Repositories
{
    public interface IUserRepo : IRepoBase<User>
    {
        Task<User> GetByUidAsync(string uid);
    }
}

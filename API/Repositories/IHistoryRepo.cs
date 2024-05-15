using API.Models.DB;
using API.Models.DTO;

namespace API.Repositories;

public interface IHistoryRepo : IRepoBase<UserAirportHistory>
{

    Task<HistoryList> GetUserHistory(string uid);

}
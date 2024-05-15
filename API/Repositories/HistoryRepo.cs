using API.Data;
using API.Models.DB;
using API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class HistoryRepo : RepoBase<UserAirportHistory> , IHistoryRepo
{
    public HistoryRepo(SWContext context) : base(context)
    {
    }

    public async Task<HistoryList> GetUserHistory(string uid)
    {
        var result = await _db.User
            .Where(user => user.ProviderId == uid)
            .Include(result => result.History).LastOrDefaultAsync();

        var listOfHistory = new HistoryList();

        foreach (UserAirportHistory historyItem in result.History)
        {
            
            listOfHistory.List.Add(new HistoryResponse()
            {
                DepartureICAO = historyItem.DepartureICAO,
                ArrivalICAO = historyItem.ArrivalICAO,
                TimeStamp = historyItem.TimeStamp
            });
        }


        return listOfHistory;


    }
}
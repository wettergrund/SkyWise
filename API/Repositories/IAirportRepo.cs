using API.Models.DB;

namespace API.Repositories;

public interface IAirportRepo : IRepoBase<Airport>
{

    Task<Airport> GetAirportByICAOAsync(string icao);
    Task<Airport> AddAiport(Airport model);

}
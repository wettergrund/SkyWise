using API.Models.DB;
using NetTopologySuite.Geometries;

namespace API.Repositories;

public interface IAirportRepo : IRepoBase<Airport>
{

    Task<Airport> GetAirportByICAOAsync(string icao);
    Task<Airport> AddAiport(Airport model);

    Task<List<Airport>> GetAirportsByLine(LineString line);

}
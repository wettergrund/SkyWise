using API.Models;
using API.Models.DB;
using API.Models.DTO;
using NetTopologySuite.Geometries;

namespace API.Repositories;

public interface IAirportRepo : IRepoBase<Airport>
{

    Task<Airport> GetAirportByICAOAsync(string icao);
    Task<Airport> AddAiport(Airport model);

    Task<List<AirportListResponse>> GetAirportsByLine(LineString line);

}
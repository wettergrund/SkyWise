using API.Data;
using API.Models.DB;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace API.Repositories;

public class AirportRepo : RepoBase<Airport>, IAirportRepo
{
    public AirportRepo(SWContext context) : base(context)
    {
    }

    public async Task<Airport> GetAirportByICAOAsync(string icao)
    {
        var response = _db.Airport.Where(ap => ap.ICAO == icao).FirstOrDefault();
        return response;
    }

    public async Task<Airport> AddAiport(Airport model)
    {
        try
        {
            _db.Airport.Add(model);
            _db.SaveChanges();
        }
        catch
        {
            throw new Exception("Unable to add airport");
            Console.WriteLine(model);
        }

        return model;
    }

    public async Task<List<Airport>> GetAirportsByLine(LineString line)
    {
        var result = _db.Airport
            .Where(ap => ap.Location.IsWithinDistance(line, 80000))
            .OrderBy(r => r.Location.Distance(line.StartPoint))
            .ToList();

        return result;
    }
}
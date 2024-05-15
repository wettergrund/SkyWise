namespace WeatherHandler.Services;

public interface IWxServices
{
    public Task<bool> FetchMetar();
    public Task<bool> FetchTaf();

    public Task<bool> CleanUp();
}
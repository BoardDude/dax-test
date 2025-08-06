using WeatherApi.Models;

namespace WeatherApi.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();
        Task<WeatherForecast?> GetForecastByIdAsync(int id);
        Task<IEnumerable<WeatherForecast>> GetForecastsByCityAsync(string city);
        Task<IEnumerable<WeatherForecast>> GetForecastsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<WeatherForecast> CreateForecastAsync(CreateWeatherForecastRequest request);
        Task<WeatherForecast> UpdateForecastAsync(int id, UpdateWeatherForecastRequest request);
        Task DeleteForecastAsync(int id);
        Task<double> ConvertTemperatureAsync(double temperature, string fromScale, string toScale);
        Task<IEnumerable<WeatherAlert>> GetAlertsByCityAsync(string city);
        Task<WeatherAlert> CreateAlertAsync(WeatherAlert alert);
        Task<Dictionary<string, object>> GetWeatherStatisticsAsync();
    }
} 
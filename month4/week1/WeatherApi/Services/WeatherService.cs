using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly List<WeatherForecast> _forecasts;
        private readonly List<WeatherAlert> _alerts;
        private int _nextForecastId = 1;
        private int _nextAlertId = 1;

        public WeatherService()
        {
            _forecasts = new List<WeatherForecast>();
            _alerts = new List<WeatherAlert>();

            // Seed with sample data
            SeedSampleData();
        }

        public Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return Task.FromResult(_forecasts.AsEnumerable());
        }

        public Task<WeatherForecast?> GetForecastByIdAsync(int id)
        {
            var forecast = _forecasts.FirstOrDefault(f => f.Id == id);
            return Task.FromResult(forecast);
        }

        public Task<IEnumerable<WeatherForecast>> GetForecastsByCityAsync(string city)
        {
            var forecasts = _forecasts.Where(f => f.City.Equals(city, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(forecasts);
        }

        public Task<IEnumerable<WeatherForecast>> GetForecastsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var forecasts = _forecasts.Where(f => f.Date >= startDate && f.Date <= endDate);
            return Task.FromResult(forecasts);
        }

        public Task<WeatherForecast> CreateForecastAsync(CreateWeatherForecastRequest request)
        {
            var forecast = new WeatherForecast
            {
                Id = _nextForecastId++,
                City = request.City,
                Date = request.Date,
                TemperatureC = request.TemperatureC,
                Description = request.Description,
                Humidity = request.Humidity,
                WindSpeed = request.WindSpeed,
                WindDirection = request.WindDirection,
                Pressure = request.Pressure,
                Visibility = request.Visibility,
                CreatedAt = DateTime.UtcNow
            };

            _forecasts.Add(forecast);
            return Task.FromResult(forecast);
        }

        public Task<WeatherForecast> UpdateForecastAsync(int id, UpdateWeatherForecastRequest request)
        {
            var forecast = _forecasts.FirstOrDefault(f => f.Id == id);
            if (forecast == null)
            {
                throw new ArgumentException("Forecast not found.");
            }

            forecast.TemperatureC = request.TemperatureC;
            forecast.Description = request.Description;
            forecast.Humidity = request.Humidity;
            forecast.WindSpeed = request.WindSpeed;
            forecast.WindDirection = request.WindDirection;
            forecast.Pressure = request.Pressure;
            forecast.Visibility = request.Visibility;

            return Task.FromResult(forecast);
        }

        public Task DeleteForecastAsync(int id)
        {
            var forecast = _forecasts.FirstOrDefault(f => f.Id == id);
            if (forecast == null)
            {
                throw new ArgumentException("Forecast not found.");
            }

            _forecasts.Remove(forecast);
            return Task.CompletedTask;
        }

        public Task<double> ConvertTemperatureAsync(double temperature, string fromScale, string toScale)
        {
            // First convert to Celsius
            double celsius = fromScale.ToUpper() switch
            {
                "C" => temperature,
                "F" => (temperature - 32) * 5.0 / 9.0,
                "K" => temperature - 273.15,
                _ => throw new ArgumentException($"Invalid scale: {fromScale}")
            };

            // Then convert from Celsius to target scale
            double result = toScale.ToUpper() switch
            {
                "C" => celsius,
                "F" => celsius * 9.0 / 5.0 + 32,
                "K" => celsius + 273.15,
                _ => throw new ArgumentException($"Invalid scale: {toScale}")
            };

            return Task.FromResult(result);
        }

        public Task<IEnumerable<WeatherAlert>> GetAlertsByCityAsync(string city)
        {
            var alerts = _alerts.Where(a => a.City.Equals(city, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(alerts);
        }

        public Task<WeatherAlert> CreateAlertAsync(WeatherAlert alert)
        {
            alert.Id = _nextAlertId++;
            alert.CreatedAt = DateTime.UtcNow;
            _alerts.Add(alert);
            return Task.FromResult(alert);
        }

        public Task<Dictionary<string, object>> GetWeatherStatisticsAsync()
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalForecasts"] = _forecasts.Count,
                ["TotalAlerts"] = _alerts.Count,
                ["Cities"] = _forecasts.Select(f => f.City).Distinct().Count(),
                ["AverageTemperature"] = _forecasts.Any() ? _forecasts.Average(f => f.TemperatureC) : 0,
                ["TemperatureRange"] = _forecasts.Any() ? new { Min = _forecasts.Min(f => f.TemperatureC), Max = _forecasts.Max(f => f.TemperatureC) } : null
            };

            return Task.FromResult(stats);
        }

        private void SeedSampleData()
        {
            var cities = new[] { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix" };
            var descriptions = new[] { "Sunny", "Cloudy", "Rainy", "Snowy", "Partly Cloudy" };
            var windDirections = new[] { "N", "S", "E", "W", "NE", "NW", "SE", "SW" };

            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var forecast = new WeatherForecast
                {
                    Id = _nextForecastId++,
                    City = cities[random.Next(cities.Length)],
                    Date = DateTime.Now.AddDays(random.Next(-7, 7)),
                    TemperatureC = random.Next(-20, 40),
                    Description = descriptions[random.Next(descriptions.Length)],
                    Humidity = random.Next(30, 90),
                    WindSpeed = random.Next(0, 50),
                    WindDirection = windDirections[random.Next(windDirections.Length)],
                    Pressure = random.Next(980, 1030),
                    Visibility = random.Next(5, 25),
                    CreatedAt = DateTime.UtcNow
                };

                _forecasts.Add(forecast);
            }

            // Add some sample alerts
            var alert = new WeatherAlert
            {
                Id = _nextAlertId++,
                City = "New York",
                Type = "Severe Thunderstorm",
                Description = "Severe thunderstorm warning in effect",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                Severity = "Warning",
                CreatedAt = DateTime.UtcNow
            };

            _alerts.Add(alert);
        }
    }
} 
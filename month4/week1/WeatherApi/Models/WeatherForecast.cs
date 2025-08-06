namespace WeatherApi.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string Description { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Pressure { get; set; }
        public double Visibility { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class WeatherAlert
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Severity { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CreateWeatherForecastRequest
    {
        public string City { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Pressure { get; set; }
        public double Visibility { get; set; }
    }

    public class UpdateWeatherForecastRequest
    {
        public double TemperatureC { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Pressure { get; set; }
        public double Visibility { get; set; }
    }
} 
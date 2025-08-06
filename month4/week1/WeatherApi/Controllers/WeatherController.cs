using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        // GET: api/weather
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetForecasts()
        {
            try
            {
                var forecasts = await _weatherService.GetAllForecastsAsync();
                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forecasts");
                return StatusCode(500, "An error occurred while retrieving forecasts");
            }
        }

        // GET: api/weather/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> GetForecast(int id)
        {
            try
            {
                var forecast = await _weatherService.GetForecastByIdAsync(id);
                if (forecast == null)
                {
                    return NotFound($"Forecast with ID {id} not found");
                }

                return Ok(forecast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forecast with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the forecast");
            }
        }

        // GET: api/weather/city/New York
        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetForecastsByCity(string city)
        {
            try
            {
                var forecasts = await _weatherService.GetForecastsByCityAsync(city);
                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forecasts for city {City}", city);
                return StatusCode(500, "An error occurred while retrieving forecasts");
            }
        }

        // GET: api/weather/range?startDate=2024-01-01&endDate=2024-01-31
        [HttpGet("range")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetForecastsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var forecasts = await _weatherService.GetForecastsByDateRangeAsync(startDate, endDate);
                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forecasts for date range {StartDate} to {EndDate}", startDate, endDate);
                return StatusCode(500, "An error occurred while retrieving forecasts");
            }
        }

        // POST: api/weather
        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> CreateForecast(CreateWeatherForecastRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var forecast = await _weatherService.CreateForecastAsync(request);
                return CreatedAtAction(nameof(GetForecast), new { id = forecast.Id }, forecast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating forecast");
                return StatusCode(500, "An error occurred while creating the forecast");
            }
        }

        // PUT: api/weather/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForecast(int id, UpdateWeatherForecastRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var forecast = await _weatherService.UpdateForecastAsync(id, request);
                return Ok(forecast);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating forecast with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the forecast");
            }
        }

        // DELETE: api/weather/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForecast(int id)
        {
            try
            {
                await _weatherService.DeleteForecastAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting forecast with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the forecast");
            }
        }

        // GET: api/weather/convert?temperature=25&fromScale=C&toScale=F
        [HttpGet("convert")]
        public async Task<ActionResult<object>> ConvertTemperature(
            [FromQuery] double temperature,
            [FromQuery] string fromScale,
            [FromQuery] string toScale)
        {
            try
            {
                var convertedTemperature = await _weatherService.ConvertTemperatureAsync(temperature, fromScale, toScale);
                return Ok(new
                {
                    OriginalTemperature = temperature,
                    FromScale = fromScale,
                    ToScale = toScale,
                    ConvertedTemperature = convertedTemperature
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting temperature");
                return StatusCode(500, "An error occurred while converting temperature");
            }
        }

        // GET: api/weather/alerts/New York
        [HttpGet("alerts/{city}")]
        public async Task<ActionResult<IEnumerable<WeatherAlert>>> GetAlertsByCity(string city)
        {
            try
            {
                var alerts = await _weatherService.GetAlertsByCityAsync(city);
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alerts for city {City}", city);
                return StatusCode(500, "An error occurred while retrieving alerts");
            }
        }

        // POST: api/weather/alerts
        [HttpPost("alerts")]
        public async Task<ActionResult<WeatherAlert>> CreateAlert(WeatherAlert alert)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdAlert = await _weatherService.CreateAlertAsync(alert);
                return CreatedAtAction(nameof(GetAlertsByCity), new { city = createdAlert.City }, createdAlert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert");
                return StatusCode(500, "An error occurred while creating the alert");
            }
        }

        // GET: api/weather/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetStatistics()
        {
            try
            {
                var statistics = await _weatherService.GetWeatherStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather statistics");
                return StatusCode(500, "An error occurred while retrieving statistics");
            }
        }
    }
} 
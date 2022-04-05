using System.Collections.Generic;
using UV_Backend.Entities.Responses.Uv.Forecast;

namespace UV_Backend.Entities.Responses.Uv
{
    public class UVForecastResponse
    {
        public List<ForecastCurrentTime> hourly { get; set; }
    }
}

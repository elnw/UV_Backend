using System;
using System.ComponentModel.DataAnnotations;

namespace UV_Backend.Models
{
    public class UV
    {
        [Key]
        public DateTime HourForecast { get; set; }

        public float Uvi { get; set; }

    }
}

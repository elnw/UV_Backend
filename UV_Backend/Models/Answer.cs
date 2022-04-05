using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace UV_Backend.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public int Point { get; set; }

    }
}

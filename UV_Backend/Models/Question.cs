using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace UV_Backend.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string question { get; set; }
    }
}

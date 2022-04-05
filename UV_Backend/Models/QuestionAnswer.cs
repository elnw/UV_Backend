using System.ComponentModel.DataAnnotations;
namespace UV_Backend.Models
{
    public class QuestionAnswer
    {
        [Key]
        public int Id { get; set; }

        public Question Question { get; set; }
        public Answer Answer { get; set; }

    }
}

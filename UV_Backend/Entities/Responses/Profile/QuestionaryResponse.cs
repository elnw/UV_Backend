using System.Collections.Generic;
using UV_Backend.Models;

namespace UV_Backend.Entities.Responses.Profile
{
    public class QuestionaryResponse
    {
        public Question Question { get; set; }
        public List<Answer> PossibleAnswers { get; set; }
    }
}

using System.Collections.Generic;

namespace UV_Backend.Entities.Responses.Profile
{
    public class ProfileInfoResponse
    {
        public float ExposureTime { get; set; }
        public List<string> falseAdvices { get; set; }
        public List<string> truthAdvices { get; set; }
        public float uvi { get; set; }
    }
}

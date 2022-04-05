namespace UV_Backend.Entities.Requests.Profile
{
    public class CreateProfileRequest
    {
        public string UserId { get; set; }
        public Models.Profile Profile { get; set; }
    }
}

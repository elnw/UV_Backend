namespace UV_Backend.Entities.Requests.Auth
{
    public class ForgotPasswordRequest
    {
        public string email { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.email);
        }
    }
}

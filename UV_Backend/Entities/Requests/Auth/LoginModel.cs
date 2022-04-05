namespace UV_Backend.Entities.Auth
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}

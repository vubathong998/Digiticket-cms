namespace Services.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public int Expires { get; set; }
        public string RefreshToken { get; set; }
    }
}

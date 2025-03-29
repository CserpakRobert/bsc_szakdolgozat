namespace Backend.Models.DTOs
{
    public class LoginDetails(string token, string userName)
    {
        public string Token { get; set; } = token;
        public string UserName { get; set; } = userName;
    }
    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterModel
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

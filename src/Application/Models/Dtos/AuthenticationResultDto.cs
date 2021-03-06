namespace Application.Models.Dtos
{
    public class AuthenticationResultDto
    {
        public bool Success { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
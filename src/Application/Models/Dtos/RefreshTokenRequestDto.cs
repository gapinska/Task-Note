namespace Application.Models.Dtos
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
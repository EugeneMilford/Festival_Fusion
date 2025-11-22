namespace FestivalFusion.API.Models.DTO
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string[]? Roles { get; set; } 
    }
}

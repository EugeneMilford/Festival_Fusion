namespace FestivalFusion.API.Models.DTO
{
    public class AddArtistRequestDto
    {
        public string Name { get; set; }
        public string ArtistImageUrl { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public string Bio { get; set; }
    }
}

namespace FestivalFusion.API.Models.DTO
{
    public class AddVenueRequestDto
    {
        public string Name { get; set; }
        public string VenueImageUrl { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
    }
}

namespace FestivalFusion.API.Models.DTO
{
    public class CreateFestivalRequestDto
    {
        public string FestivalName { get; set; }
        public string FestivalImageUrl { get; set; }
        public string FestivalDescription { get; set; }
        public string Theme { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Sponsor { get; set; }
    }
}

namespace FestivalFusion.API.Models.DTO
{
    public class UpdateBlogRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
    }
}

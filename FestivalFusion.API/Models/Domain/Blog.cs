namespace FestivalFusion.API.Models.Domain
{
    public class Blog
    {
        public int BlogId { get; set; }

        public string Title { get; set; } 
        public string Content { get; set; }
        public string Category { get; set; }
        public string FeaturedImageUrl { get; set; } 
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
    }
}

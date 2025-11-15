export interface UpdateBlogRequest{
    title: string,
    content: string, 
    category: string,
    featuredImageUrl: string,
    author: string,
    publishedDate: string | Date | null,
    updatedDate: string
    isPublished: boolean;
    isFeatured: boolean;
}
export interface AddBlogRequest{
    title: string;
    content?: string;                 
    category?: string;
    featuredImageUrl?: string;
    author?: string;
    publishedDate?: string | Date | null;
    updatedDate?: string | Date | null;
    isPublished: boolean;
    isFeatured: boolean;
}
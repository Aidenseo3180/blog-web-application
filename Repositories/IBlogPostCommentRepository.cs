using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        //return BlogPostComment
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        //return list of BlogPostComment
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);

    }
}

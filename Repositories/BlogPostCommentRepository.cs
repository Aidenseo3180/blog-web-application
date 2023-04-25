using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostCommentRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            //go to blogDb, choose BlogPostComments, call AddAsync function of that table
            await blogDbContext.BlogPostComments.AddAsync(blogPostComment);
            await blogDbContext.SaveChangesAsync();

            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await blogDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

    }
}

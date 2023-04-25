using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();

        //get single blogpost
        Task<BlogPost?> GetAsync(Guid id);

        //For blogposts from dashboard, takes urlHandle
        Task<BlogPost?> GetByUrlHandleAsync(string UrlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}

using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogDbContext.AddAsync(blogPost); //AddAsync from Microsoft Core
            await blogDbContext.SaveChangesAsync();
            return blogPost; 
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            //first check if it exists
            var existingBlog = await blogDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                blogDbContext.BlogPosts.Remove(existingBlog);
                await blogDbContext.SaveChangesAsync();

                //return deleted blog post
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()    //for List
        {
            //Call BlogPosts DB from blogDbContext, we want to give whole list back to controller, so ToList to send whole table in List
            //Use include to get property we want (Tags in this case)
            return await blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)    //get single blogpost using id
        {
            //return found blogpost that has same id
            //if not found, return null
            //We also want to include Tags in this particular blogpost
            return await blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string UrlHandle)
        {
            //include both the tag and blogpost with same urlhandle
            return await blogDbContext.BlogPosts.Include(x => x.Tags).
                FirstOrDefaultAsync(x => x.UrlHandle == UrlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            //compare with the database
            //Find existing blog with id, Read from database
            var existingBlog = await blogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            //if found, update all the existing values
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeatureImageURL = blogPost.FeatureImageURL;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Author = blogPost.Author;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.Tags = blogPost.Tags;

                await blogDbContext.SaveChangesAsync(); //save changes to database

                return existingBlog;
            }
            return null;
        }
    }
}

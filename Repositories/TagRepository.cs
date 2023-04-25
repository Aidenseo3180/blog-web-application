using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        //Implementation of interface
        //drag mouse over ITagInterface, ctrl + . and click implement interface

        private readonly BlogDbContext blogDbContext;

        //constructor injection, inject BlogDbContext into constructor. ctor double tab for shortcut
        public TagRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            //adding new tag(values) inside Tags table inside SQL server. Add is .NET Core function, not from our code
            await blogDbContext.Tags.AddAsync(tag);
            //blogDbContext save changes to database
            await blogDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existing_tag = await blogDbContext.Tags.FindAsync(id);
            if (existing_tag != null)
            {
                //remove found entity
                //remove doesn't have asyn, so keep
                blogDbContext.Tags.Remove(existing_tag);
                await blogDbContext.SaveChangesAsync();
                
                return existing_tag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() //for List
        {
            //use dbcontext to read tags database
            //now it returns list of tags database (the whole table)
            return await blogDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            //FirstOrDefault returns first one it finds (that meets x.Id == id)
            //x is the arbitary name for each row
            return blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existing_tag = await blogDbContext.Tags.FindAsync(tag.Id);

            if (existing_tag != null)
            {
                //change the Name and DisplayName of found table
                existing_tag.Name = tag.Name;
                existing_tag.DisplayName = tag.DisplayName;

                //save changes
                await blogDbContext.SaveChangesAsync();

                return existing_tag;
            }
            //return null when unsuccessful
            return null;
        }
    }
}

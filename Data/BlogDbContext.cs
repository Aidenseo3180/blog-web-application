using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    //inherit DbContext from Microsoft EntityFrameworkCore
    public class BlogDbContext : DbContext
    {
        //These are tables in SQL
        //since we have 2 Db, specific with <>. When there's only 1, don't need to
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        //below are tables in DBContext database (we create them manually & use .NET commands in terminal to put them in DB)
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogPostComment> BlogPostComments { get; set; }

    }
}

using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        //To get list of user from database, use AuthDb
        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            //access Users table, get list
            var users = await authDbContext.Users.ToListAsync();

            //remove superadmin from list
            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@blog.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }

            return users;
        }
    }
}

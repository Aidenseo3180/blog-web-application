using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Repositories
{
    public interface IUserRepository
    {
        //return list of IdentityUser
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}

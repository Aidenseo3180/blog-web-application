using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface ITagRepository
    {
        //has only methods (like abstract methods)
        //since we want CRUD operations, make methods for all (create, add, update, delete, etc.)
        //use async 

        //returns domain model because BlogDbContext only knows about Domain, and not ViewModels (it only import Domain)
        //IEnumerable to get ALL the tags
        Task<IEnumerable<Tag>> GetAllAsync();   //since async, wrap return with Task<>

        //For single Tag, takes id as parameter to determine which tag to get
        Task<Tag?> GetAsync(Guid id);

        //AddAsync need entire Tag object to save and create new rule in DB
        //so take tag as input, return changed tag
        Task<Tag> AddAsync(Tag tag);

        //Since it search if id is available, so return can be nullable
        Task<Tag?> UpdateAsync(Tag tag);

        //Delete using id, return nullable if id match not found
        Task<Tag?> DeleteAsync(Guid id);


    }
}

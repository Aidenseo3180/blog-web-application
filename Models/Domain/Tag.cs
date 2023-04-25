namespace Blog.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        //Tags can have multiple BlogPost (Connection made)
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}

namespace Blog.Web.Models.Domain
{
    public class BlogPost
    {
        //These are what BlogPost can have
        public Guid Id { get; set; }
        //ignore warning. Since we don't want these like heading to have both the value and null, we don't put question mark
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeatureImageURL { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //1 blog can have many Tag, so ICollection
        //BlogPost can have multiple Tag
        //Navigation property - tells entity framework that this is related property
        public ICollection<Tag> Tags { get; set; }
        public ICollection<BlogPostComment> Comments { get; set; }
    }
}

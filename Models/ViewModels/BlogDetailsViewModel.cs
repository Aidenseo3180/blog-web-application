using Blog.Web.Models.Domain;

namespace Blog.Web.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
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

        //no longer navigation property
        public ICollection<Tag> Tags { get; set; }

        public string CommentDescription { get; set; }  //need to add comment 

        //IEnumerable = same as list (but used for return types). So it returns list of BlogComment
        public IEnumerable<BlogComment> Comments { get; set; }
    }
}

using Blog.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Models.ViewModels
{
    public class EditBlogPostRequest
    {
        //use nullible? if you don't want to make them required
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeatureImageURL { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //for post dropdown box
        //Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        //capture tags
        //return array to allow it to select multiple tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

    }
}

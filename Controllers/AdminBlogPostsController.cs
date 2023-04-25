using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        //constructor injection
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get all tags from repository (database)
            var tags = await tagRepository.GetAllAsync();

            //now assign what we have to view model
            var model = new AddBlogPostRequest
            {
                //Make tag out of selected values
                //Text is the display, value for value of the name
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        //display list of blog posts on webpage
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            try
            {

                //map view model to domain because AddAsync takes domain
                var blogPost = new BlogPost
                {
                    Heading = addBlogPostRequest.Heading,
                    PageTitle = addBlogPostRequest.PageTitle,
                    Content = addBlogPostRequest.Content,
                    ShortDescription = addBlogPostRequest.ShortDescription,
                    FeatureImageURL = addBlogPostRequest.FeatureImageURL,
                    UrlHandle = addBlogPostRequest.UrlHandle,
                    PublishedDate = addBlogPostRequest.PublishedDate,
                    Author = addBlogPostRequest.Author,
                    Visible = addBlogPostRequest.Visible,
                };

                //temporary variable to keep all tags we want
                var selectedTags = new List<Tag>();

                //Since blogPost obj above needs tags,find it using selected tags from addBlogPostRequest
                foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
                {
                    //since we need to pass id in guid to GetAsync, parse selectedTagId as Guid
                    var selectedTagIdAsGuid = Guid.Parse(selectedTagId);

                    //get tag from Guid Id
                    var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                    //if existing Tag found
                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }

                //after finding all selectedTags, add it to blogPost.Tags
                blogPost.Tags = selectedTags;
                await blogPostRepository.AddAsync(blogPost);

                TempData["AlertMessage"] = "Blogpost added successfully.";

                return RedirectToAction("List");
            }
            catch
            {
                TempData["DangerMessage"] = "Blogpost added unsuccessfully. Please try again.";
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call the repository and get data
            //Even though blogPostRepo is interface, since we used AddScoped to return its implementation from Program.cs, it works
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //retrieve result from repository (repo retrieve from database)
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                //map domain model into view model so that we can send to View
                //basically, read from domain and send to view
                //because we have to show what's in database to user (so that user can see and update manually)
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeatureImageURL = blogPost.FeatureImageURL,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),  //convert to ToString b/c we want to display it
                         //use SelectListItem b/c Tags is List of SelectListItem
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray() // convert to string and convert to array
                };

                return View(model); //AdminBlogPosts -> Edit receives EditBlogPostRequest model
            }

            return View(null);
        }

        [HttpPost] // When Update button is pressed from AdminBlogPosts - Edit View
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            try
            {
                //map view model back to domain model
                var blogPostDomainModel = new BlogPost
                {
                    Id = editBlogPostRequest.Id,
                    Heading = editBlogPostRequest.Heading,
                    PageTitle = editBlogPostRequest.PageTitle,
                    Content = editBlogPostRequest.Content,
                    ShortDescription = editBlogPostRequest.ShortDescription,
                    FeatureImageURL = editBlogPostRequest.FeatureImageURL,
                    UrlHandle = editBlogPostRequest.UrlHandle,
                    PublishedDate = editBlogPostRequest.PublishedDate,
                    Author = editBlogPostRequest.Author,
                    Visible = editBlogPostRequest.Visible,
                };


                //map tags separately, map tags into domain model
                var selectedTags = new List<Tag>();
                foreach (var selectedTag in editBlogPostRequest.SelectedTags)
                {
                    //if we can parse selectedTag and out var tag
                    if (Guid.TryParse(selectedTag, out var tag))
                    {
                        var foundTag = await tagRepository.GetAsync(tag);

                        if (foundTag != null)
                        {
                            selectedTags.Add(foundTag);
                        }
                    }
                }
                blogPostDomainModel.Tags = selectedTags;

                //submit information to repository to update (then repo updates database)
                var EditBlogPost = await blogPostRepository.UpdateAsync(blogPostDomainModel);

                //show success notification
                TempData["AlertMessage"] = "Blogpost updated successfully.";
                return RedirectToAction("List");
            }
            catch
            {
                //show error notification
                TempData["AlertMessage"] = "Blogpost updated unsuccessfully. Please try again.";
                return RedirectToAction("Edit");
            }
        }

        [HttpPost]  //Post b/c we're posting information from the model
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            try
            {
                //Talk to repository to delete this blog post and tags
                var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
     
                //show success notification
                TempData["AlertMessage"] = "Blogpost deleted successfully.";
                return RedirectToAction("List");    //go back to list page
            }
            catch
            {
                //show error notification
                TempData["AlertMessage"] = "Blogpost deleted unsuccessfully. Please try again.";

                //go to Edit(Guid id), since it takes parameter, give id
                return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
            }
        }

    }
}

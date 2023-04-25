using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(
            IBlogPostRepository blogPostRepository,
            IBlogPostCommentRepository blogPostCommentRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)  //bring in blogPost repo b/c we will use it
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        //route coming in, which is a URL handle
        //When blog clicked from Index.cshtml, 
        [HttpGet]
        public async Task<IActionResult> Index(string UrlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(UrlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();  //since we want to keep it outside of if statement,

            if (blogPost != null)
            {
                //get comments for blog post
                var blogCommentDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();
                
                foreach(var blogComment in blogCommentDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                        //Find username using id from userManager, convert it to string(bc its guid)
                        //once we find it, give it UserName
                    });
                }
                

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeatureImageURL = blogPost.FeatureImageURL,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailsViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            //when form is submitted from blog index.cshtml, BlogDetailsViewModel also passed back (so take as parameter)

            if (signInManager.IsSignedIn(User)) //if signed in
            {
                var domainModel = new BlogPostComment
                {
                    //dont give id bc entity framework will generate one for us
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),   //returns string, so convert to Guid
                    DateAdded = DateTime.Now
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", 
                    new { urlHandle = blogDetailsViewModel.UrlHandle}); //select same blog again using urlhandle
            }
            return View(); //redirect backs to same page (refresh & stays in blog)
        }
    }
}

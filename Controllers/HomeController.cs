using Blog.Web.Models;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;    //repo for blogPost data
        private readonly ITagRepository tagRepository;  //repo for tag data

        //To retrieve blogs data from database, use repository
        //for tag use tagRepo
        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, 
            ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            //get all blogs from repo
            var blogPosts = await blogPostRepository.GetAllAsync();

            //get all tags from repo
            var tags = await tagRepository.GetAllAsync();

            //To call 2 Models from same View: have 1 view that combines these 2 different views
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);
        }

        public async Task<IActionResult> AllPost()
        {
            //get all blogs from repo
            var blogPosts = await blogPostRepository.GetAllAsync();

            //get all tags from repo
            var tags = await tagRepository.GetAllAsync();

            //To call 2 Models from same View: have 1 view that combines these 2 different views
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);
        }

        public IActionResult AboutMe()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
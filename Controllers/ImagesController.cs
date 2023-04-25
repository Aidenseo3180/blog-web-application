using Blog.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

//API controller, so doesn't expect View to be returned
namespace Blog.Web.Controllers 
{
    [Route("api/[controller]")] //controller is where ImagesController goes
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST take image, upload to cloudhosting platform using repository
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file) //IFormFile: class we use to upload files
        {
            //call repository
            var imageURL = await imageRepository.UploadAsync(file);

            //if URL not found, 
            if (imageURL == null)
            {
                //display string, give null, return 500
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            }

            //when successful, send response(imageURL) back in json
            return new JsonResult(new {link=imageURL});
        }

    }
}

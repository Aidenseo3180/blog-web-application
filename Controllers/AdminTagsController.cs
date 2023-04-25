using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//press control space to see parameters
namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]    //all the methods here require to log in & only admin can access
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        //Inject ITagRepository using constructor injection
        //drag mouse to tagRepository + ctrl + . to create assigned field
        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add() //display page back to the user
        {
            return View();  //Make View cshtml file that has same name as this method name
        }

        //HttpPost because when you submit the form by pressing button, it goes to post method from add.cshtml
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)   //input coming from ViewModels folder
        {
            if (ModelState.IsValid == false)
            {
                TempData["AlertMessage"] = "Tag added unsuccessfully. Please try again.";
                return View();
            }

            try
            {
                //mapping AddTagRequest to Tag Domain model
                //convert to Domain from ViewModel (both are models)
                //mapping doesn't matter to databse, so not async
                var tag = new Tag()
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName
                };
                //read from repository (and repository reads from database)
                //Good for security
                var state = await tagRepository.AddAsync(tag);

                TempData["AlertMessage"] = "Tag added successfully.";
                return RedirectToAction("List");

            }
            catch
            {
                TempData["AlertMessage"] = "Tag added unsuccessfully. Please try again.";
                return View();
            }
        }

        //HtppGet method to retrieve information from another page
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)  //name of parameter has to match with asp-route(id in this case) that we created
        {
            
            var tag = await tagRepository.GetAsync(id);

            //if tag was found
            if (tag != null)
            {
                //EditTagRequest comes from view model
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                //now provide editTagRequest above to View to populate it
                //View can take object arguments, and editTagRequest is an object
                return View(editTagRequest);
            }

            return View(null);  //if not found, return null (null is also an object to possible)
        }

        //for update button in edit
        //edit in post method b/c it submits form
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            try
            {
                //convert back into Domain model from ViewModel
                var tag = new Tag
                {
                    Id = editTagRequest.Id,
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName
                };

                var updated_tag = await tagRepository.UpdateAsync(tag);

                //show success notification
                TempData["AlertMessage"] = "Tag updated successfully.";
                return RedirectToAction("List");
            }
            catch
            {
                TempData["AlertMessage"] = "Tag updated unsuccessfully. Please try again.";
                //Since Edit takes parameter, use new
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
        }

        //since submitting form with button, it also takes editTagRequest
        //since submitting form, also post
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            try
            {
                var deleted_tag = await tagRepository.DeleteAsync(editTagRequest.Id);

                //show success notification
                TempData["AlertMessage"] = "Tag deleted successfully.";

                //Then redirect to List
                return RedirectToAction("List");
            }
            catch
            {
                //show unsuccess notification
                TempData["AlertMessage"] = "Tag deleted unsuccessfully. Please try again.";
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
        }
    }
}

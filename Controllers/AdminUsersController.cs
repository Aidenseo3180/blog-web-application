using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        //show all users
        [HttpGet]
        public async Task<IActionResult> List()     //gets called when ../AdminUsers/List
        {
            var users = await userRepository.GetAll();

            //Since we don't want to pass Domain model to View(), make ViewModel and pass it
            //UserViewModel has list of User
            //User has 3 properties: Id, email, username
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            //iterate each user in users list
            foreach(var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }

            return View(usersViewModel);
        }

        //Gets called after you make changes to user and come back to List (takes argument of changed UserViewModel)
        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)   
        {
            //similar to Register, create account
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityResult = 
                await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult != null)
            {

                if (identityResult.Succeeded)
                {
                    //assign roles to user
                    var roles = new List<string> { "User" };    //user by default
                    @TempData["AlertMessage"] = "User added successfully.";
                    //if adminRoleCheck is true
                    if (request.AdminRoleCheck)
                    {
                        @TempData["AlertMessage"] = "Admin added successfully.";
                        roles.Add("Admin");
                    }

                    identityResult =
                        await userManager.AddToRolesAsync(identityUser, roles);    //now assign roles to user

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }

                }
                else // if not successful, delete it b/c it fails
                {
                    await userManager.DeleteAsync(identityUser);
                    @TempData["DangerMessage"] = "User added unsuccessfully. Please double check your password";
                }
                
            }
            return RedirectToAction("List", "AdminUsers");
        }

        //get Guid id from asp-route of delete button
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult != null &&  identityResult.Succeeded)
                {
                    @TempData["AlertMessage"] = "User deleted successfully.";
                    return RedirectToAction("List", "AdminUsers");
                }

            }
            @TempData["DangerMessage"] = "User deleted unsuccessfully. Please try again.";
            return RedirectToAction("List", "AdminUsers");
        }
    }
}

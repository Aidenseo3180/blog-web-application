using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager; //sign in manager helps to sign in

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //Goes from Get --> Post. So Get to reach Register page, then Post Register to send data out of register
        //so Post Register() gets called when form is submitted (or submit is called)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]  //user info comes in, create new user
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //custom validation
            ValidateAccount(registerViewModel);

            //use ModelState to check if RegisterViewModel is in a correct state or not
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                //assign user role to newly created user
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityResult.Succeeded)
                {
                    //redirect to login page
                    return RedirectToAction("Login", "Account");
                }
            }

            //if all the requirements meet (except for unique character)
            ModelState.AddModelError("Password", "Password must contain at least 1 unique character");
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)    //save url, return later
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                //takes username and password in string, etc.
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,
                    false, false);

                if (signInResult != null && signInResult.Succeeded)
                {
                    //check ReturnUrl. If exists, return there
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                TempData["AlertMessage"] = "Login unsuccessful. Please try again.";
                return View();
            }
            TempData["AlertMessage"] = "Login unsuccessful. Please try again.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() //gets called when access is denied
        {
            return View();
        }

        private void ValidateAccount(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != null)
            {
                var password = registerViewModel.Password;
                if (password.Any(char.IsUpper) == false)
                {
                    //Say password so that it prints below password input section
                    ModelState.AddModelError("Password", "Password must contain at least 1 upper case");
                }
                if (password.Any(char.IsLower) == false)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 1 lower case");
                }
                if (password.Any(char.IsDigit) == false)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 1 digit number");
                }
                if (password.Any(char.IsDigit) == false)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 1 digit");
                }
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Delete()
        //{
        //    userManager.DeleteAsync()
        //    return RedirectToAction("Index", "Home");
        //}

    }
}

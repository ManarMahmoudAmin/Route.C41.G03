using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.C41.G03.DAL.Models;
using Route.C41.G03.PL.ViewModels.Account;
using System.Threading.Tasks;

namespace Route.C41.G03.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user is null)
				{
					var flag =await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

						if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account Is Locked!");


						if (result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");

						if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet!");
					}

				}
				ModelState.AddModelError(string.Empty, "Invalid Login");


			}
			return View(model);
		}
	}
}

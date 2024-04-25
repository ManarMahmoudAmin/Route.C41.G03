using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Route.C41.G03.DAL.Models;
using Route.C41.G03.PL.Services;
using Route.C41.G03.PL.ViewModels.Account;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;


namespace Route.C41.G03.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

		public AccountController(
					IEmailSender emailSender,
					IConfiguration configuration,
					UserManager<ApplicationUser> userManager,
					SignInManager<ApplicationUser> signInManager)
		{
			_emailSender = emailSender;
			_configuration = configuration;
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

		#region Forget Password
		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model) 
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var ResetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = ResetPasswordToken }, "https://localhost:44331/");

					await _emailSender.SendAsync(
						from: _configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
						subject: "Reset Your Password",
						body: resetPasswordUrl);
					return RedirectToAction(nameof(CheckYourInput));
				}
				ModelState.AddModelError(string.Empty, "There is No Account With This Email!!");
			}
				return View(model);
			}

		public IActionResult CheckYourInput()
		{
			return View();
		}
		#endregion
        public async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}

	}
}

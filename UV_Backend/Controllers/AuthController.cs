using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UV_Backend.Entities;
using UV_Backend.Entities.Auth;
using UV_Backend.Entities.Requests.Auth;
using UV_Backend.Entities.Responses.Auth;
using UV_Backend.Logic.Auth;
using UV_Backend.Viewmodels;

namespace UV_Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private AuthLogic _authLogic;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AuthLogic authLogic)
        {
            _authLogic = authLogic;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> Login([FromBody] LoginModel loginModel)
        {
           if(loginModel == null) return Json(new ApiResponse<LoginResponse> { message = "No se envió datos algunos" });

           if(!loginModel.IsValid()) return Json(new ApiResponse<LoginResponse> { message = "La información enviada es inválida"});

           return Json(await _authLogic.AuthenticateUser(loginModel));
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null) return Json(new ApiResponse<LoginResponse> { message = "No se envió datos algunos" });

            if (!registerRequest.IsValid()) return Json(new ApiResponse<LoginResponse> { message = "La información enviada es inválida" });

            return Json(await _authLogic.RegisterUser(registerRequest));
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> RequestPasswordChange([FromBody] ForgotPasswordRequest request)
        {

            if (request == null) return Json(new ApiResponse<LoginResponse> { message = "No se envió datos algunos" });
            if (!request.IsValid()) return Json(new ApiResponse<LoginResponse> { message = "La información enviada es inválida" });

            var user = await _authLogic.CheckIfUserExist(request.email);
            if (user == null)
                return Json(new ApiResponse<ForgotPasswordResponse> { message = "Usuario no encontrado" });

            var token = await _authLogic.RequestPasswordResetToken(user);
            var callback = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            return Json(await _authLogic.SendResetToken(callback, user.Email));
        }

        [HttpGet("[action]")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet("[action]")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}

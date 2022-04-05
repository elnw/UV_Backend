using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UV_Backend.Entities;
using UV_Backend.Entities.Auth;
using UV_Backend.Entities.Requests.Auth;
using UV_Backend.Entities.Responses.Auth;
using UV_Backend.Helpers.Email;


namespace UV_Backend.Logic.Auth
{
    public class AuthLogic
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private EmailSender _emailSender;
        public AuthLogic(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public async Task<ApiResponse<LoginResponse>> AuthenticateUser(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                return new ApiResponse<LoginResponse> { data = new LoginResponse { UserName = loginModel.UserName, UserId = user.Id } };
            }
            else
            {
                return new ApiResponse<LoginResponse> { message = "Usuario o contraseña incorrecta"};
            }

        }

        public async Task<ApiResponse<RegisterResponse>> RegisterUser(RegisterRequest registerRequest)
        {
            var user = new IdentityUser { UserName = registerRequest.UserName, Email = registerRequest.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                return new ApiResponse<RegisterResponse> { data = new RegisterResponse { Code = "200" } };
            }
            else { 
                return new ApiResponse<RegisterResponse> { message = "Ocurrieron uno o mas errores al registrar la operación", data = new RegisterResponse { Code = result.Errors.ToString() } }; 
            };

        }

        public async Task<IdentityUser> CheckIfUserExist(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);

        }

        public async Task<string> RequestPasswordResetToken(IdentityUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ApiResponse<ForgotPasswordResponse>> SendResetToken(string callbackUrl, string userEmail)
        {
            try
            {
                var message = new Message(new string[] { userEmail }, "Reset password token", callbackUrl);
                await _emailSender.SendEmailAsync(message);
                return new ApiResponse<ForgotPasswordResponse> { message = "Se mandó el mensaje con éxito" };
            }catch(Exception ex)
            {
                return new ApiResponse<ForgotPasswordResponse> { message = "Ocurrió un error al mandar el mensaje, intente nuevamente", data = new ForgotPasswordResponse { response = ex.Message } };
            }
            
        }

    }
}

using BusinessNetworking.Entities;
using BusinessNetworking.Services;
using BusinessNetworking.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BusinessNetworking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClientUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.CreatedDate = DateTime.UtcNow;

            try
            {
                var registeredUser = await _userService.RegisterUser(user);

                // Envía el correo de confirmación
                var emailHtml = $"<p>Por favor confirme su correo electrónico haciendo clic en el siguiente enlace: <a href='link-de-confirmacion'>Confirmar Correo</a></p>";
                await _emailService.SendEmailAsync(user.Email, "Confirmación de Email", emailHtml);

                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var token = await _userService.AuthenticateUser(loginModel.Email, loginModel.Password);

                if (token == null)
                {
                    return Unauthorized("Credenciales inválidas");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetExpertByUserId")]
        public async Task<IActionResult> GetExpertByUserId(int UserId)
        {
            try
            {
                var expertUser = await _userService.GetExpertByUserId(UserId);
                return Ok(new { User = expertUser });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
    }

    public class LoginModel
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Email { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Password { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    }
}

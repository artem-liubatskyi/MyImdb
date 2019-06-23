using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.ApiModels.Models;
using MyIMDB.Services;
using MyIMDB.Services.Helpers;
using MyIMDB.Web.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AuthenticationData
{
    public string login { get; set; }
    public string password { get; set; }
}
namespace MyIMDB.Web.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;
        private readonly AppSettings _appSettings;

        public AccountController(IAccountService _service, IOptions<AppSettings> appSettings)
        {
            service = _service;
            _appSettings = appSettings.Value;
        }

        [HttpGet("registration-data")]
        public async Task<IActionResult> GetRegisterViewData()
        {
            return Ok(await service.GetRegistrationData());
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationData data)
        {
            var user = await service.Authenticate(data.login, data.password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                id = user.Id,
                login = user.Login,
                fullName = user.FullName,
                token = tokenString,
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            await service.Create(model);
            return Ok();
        }

        [Authorize]
        [HttpGet("user-page")]
        public async Task<IActionResult> UserPage()
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            return Ok(await service.GetUserPageModel(userId));
        }

        [HttpPost("restore-password")]
        public async Task<IActionResult> RestorePassword([FromBody]RestorePasswordApiModel model)
        {
            await service.RestorePassword(model.newPassword, model.passwordHash);
            return Ok();
        }

        [HttpPost("restore-password-request")]
        public async Task<IActionResult> RestorePasswordRequest([FromForm]string email)
        {
            await service.ForgotPassword(email, NotificationServiceType.Email);
            return Ok();
        }
    }
}
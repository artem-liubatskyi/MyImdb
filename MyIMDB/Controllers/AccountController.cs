using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.ApiModels.Models;
using MyIMDB.Services;
using MyIMDB.Services.Helpers;
using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions options;

        public AccountController(IAccountService service, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.jwtFactory = jwtFactory ?? throw new ArgumentNullException(nameof(jwtFactory));
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
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

            var tokenString = await jwtFactory.GenerateEncodedToken(user);

            if (user.Token == null || !user.Token.Active)
            {
                var refreshToken = jwtFactory.GenerateRefreshToken(user.Id, "");
                await service.SetRefreshToken(user, refreshToken);
                return Ok(new
                {
                    id = user.Id,
                    userName = user.UserName,
                    fullName = user.FullName,
                    accessToken = tokenString,
                    refreshToken = refreshToken
                });
            }
            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                fullName = user.FullName,
                accessToken = tokenString,
                refreshToken = user.Token.Token
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
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshTokenModel model)
        {
            var principal = service.GetPrincipalFromExpiredToken(model.Token);

            var userId = Convert.ToInt64(principal.Claims.First(x => x.Type == ClaimTypes.Name).Value);

            var user = await service.Get(userId);

            if(user==null)
                throw new NullReferenceException($"No user for your token");

            if (user.Token == null)
                throw new NullReferenceException($"No tokek for {user.FullName}");

            if (user.Token.Token != model.RefreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken =
                await jwtFactory.GenerateEncodedToken(user);

            var newRefreshToken = jwtFactory.GenerateRefreshToken(userId, "");

            await service.SetRefreshToken(user, newRefreshToken);

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                fullName = user.FullName,
                accessToken = newJwtToken,
                refreshToken = newRefreshToken.Token
            });
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Services;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.Web.Helpers;

public class AuthenticationData
{
    public string login { get; set; }
    public string password { get; set; }
}
namespace MyIMDB.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService service;
        private readonly AppSettings _appSettings;

        public AccountController(IAccountService _service, IOptions<AppSettings> appSettings)
        {
            service = _service;
            _appSettings = appSettings.Value;
        }
        [AllowAnonymous]
        [HttpGet("registration-data")]
        public async Task<IActionResult> GetRegisterViewData()
        {
            return Ok(await service.GetRegistrationData());
        }
        [AllowAnonymous]
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
                id=user.Id,
                login= user.Login,
                fullName = user.FullName,
                token= tokenString,
            });
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            var user = new User()
            {
                Login = model.Login,
                FullName = model.FullName,
                EMail = model.Email,
                DateOfBirth = model.DateOfBirth,
                Biography = model.About,
                GenderId = model.GenderId,
                CountryId = model.CountryId
            };

            try
            {
                service.Create(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
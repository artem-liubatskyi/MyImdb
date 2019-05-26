using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIMDB.ApiModels.Models;
using MyIMDB.Services;

namespace MyIMDB.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieService service;
        public MoviesController(IMovieService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            long? userId=null;
            if (User.Identity.IsAuthenticated)
                userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            return Ok(await service.Get(id, userId));
        }
        [HttpGet("search/{searchQuery}")]
        public async Task<IActionResult> GetBySearchQuery(string searchQuery)
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
                userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            return Ok(await service.GetListBySearchQuery(searchQuery, userId));
        }
        [HttpGet("top")]
        public async Task<IActionResult> GetTop()
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
                userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            return Ok(await service.GetTop(userId));
        }
        [Authorize]
        [HttpPost("AddRate")]
        public async Task<IActionResult> AddRate(RateViewModel model)
        {
            await service.AddRate(model);
            return RedirectToAction("UserPage", "Account");
        }
    }
}
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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService service;
        public MoviesController(IMovieService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await service.Get(id, GetUserId()));
        }
        [HttpGet("search/{searchQuery?}")]
        public async Task<IActionResult> GetBySearchQuery(string searchQuery)
        {
            return Ok(await service.GetListBySearchQuery(searchQuery, GetUserId()));
        }
        [HttpGet("top")]
        public async Task<IActionResult> GetTop()
        {
            return Ok(await service.GetTop(GetUserId()));
        }
        [Authorize]
        [HttpPost("rate")]
        public async Task<IActionResult> AddRate([FromBody]RateViewModel model)
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            await service.AddRate(model, userId);
            return Ok();
        }
        private long? GetUserId()
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
                userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);
            return userId;
        }
    }
}
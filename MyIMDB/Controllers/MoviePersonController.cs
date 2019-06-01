using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyIMDB.Services;

namespace MyIMDB.Web.Controllers
{
    [ApiController]
    [Route("persons")]
    public class MoviePersonController : ControllerBase
    {
        private readonly IMoviePersonService service;

        public MoviePersonController(IMoviePersonService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await service.Get(id));
        }
        [HttpGet("search/{searchQuerue}")]
        public async Task<IActionResult> GetBySearchQuery(string searchQuerue)
        {
            return Ok(await service.GetListBySearchQuery(searchQuerue));
        }
    }
}
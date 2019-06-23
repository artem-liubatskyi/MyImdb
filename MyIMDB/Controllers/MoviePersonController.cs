using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Services;
using System;
using System.Threading.Tasks;

namespace MyIMDB.Web.Controllers
{
    [ApiController]
    [Route("persons")]
    public class MoviePersonController : ControllerBase
    {
        private readonly IMoviePersonService service;
        private readonly IMapper mapper;

        public MoviePersonController(IMoviePersonService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(mapper.Map<MoviePerson, MoviePersonViewModel>(await service.Get(id)));
        }
    }
}
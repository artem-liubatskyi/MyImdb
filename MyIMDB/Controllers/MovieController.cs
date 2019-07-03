using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyIMDB.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService service;
        private readonly IMapper mapper;
        private readonly ITmdbService tmdbService;

        public MoviesController(IMovieService service, IMapper mapper, ITmdbService tmdbService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.tmdbService = tmdbService ?? throw new ArgumentNullException(nameof(tmdbService));
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(long id)
        {
            var userId = GetUserId();
            var entity = await service.Get(id, userId);
            return Ok(mapper.Map<Movie, MovieViewModel>(entity, opt => opt.Items.Add("userId", userId)));
        }
        [HttpGet("search/{searchQuery?}")]
        public async Task<IActionResult> GetBySearchQuery(string searchQuery)
        {
            if (searchQuery == null || searchQuery.Length <= 3)
                return Ok();

            var userId = GetUserId();

            var entity = await service.GetListBySearchQuery(searchQuery, GetUserId());

            if (!entity.Any())
            {
                var title = await tmdbService.AddMovie(searchQuery);
                entity = await service.GetListBySearchQuery(title, userId);

                return Ok(mapper.Map<IEnumerable<Movie>, MovieListViewModel[]>(entity, opt => opt.Items.Add("userId", userId)));
            }
            return Ok(mapper.Map<IEnumerable<Movie>, MovieListViewModel[]>(entity, opt => opt.Items.Add("userId", userId)));
        }
        [HttpGet("top")]
        public async Task<IActionResult> GetTop()
        {
            var userId = GetUserId();
            var entity = await service.GetTop(userId);
            return Ok(mapper.Map<IEnumerable<Movie>, MovieListViewModel[]>(entity, opt => opt.Items.Add("userId", userId)));
        }
        [Authorize]
        [HttpPost("rate")]
        public async Task<IActionResult> AddRate([FromBody]RateViewModel model)
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            await service.AddRate(model, userId);
            return Ok();
        }
        [Authorize]
        [HttpPost("add-to-watchlist")]
        public async Task<IActionResult> AddToWatchlist([FromBody]long movieId)
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            await service.AddToWatchlist(movieId, userId);
            return Ok();
        }
        [Authorize]
        [HttpPost("remove-from-watchlist")]
        public async Task<IActionResult> RemoveFromWatchlist([FromBody]long movieId)
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.Name).Value);

            await service.RemoveFromWatchlist(movieId, userId);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyIMDB.ApiModels.Models;
using MyIMDB.Services;

namespace MyIMDB.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IMovieService movieService;
        private readonly IMoviePersonService moviePersonService;

        public SearchController(IMovieService movieService, IMoviePersonService moviePersonService)
        {
            this.movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            this.moviePersonService = moviePersonService ?? throw new ArgumentNullException(nameof(moviePersonService));
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchResults(SearchQueryViewModel model)
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
                userId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var ViewModel = new SearchResultViewModel() { searchQuery = model.searchQuery};

            if(model.Category==ECategory.Movie || model.Category == ECategory.All)
                ViewModel.Movies = await movieService.GetListBySearchQuery(model.searchQuery, userId);

            if (model.Category == ECategory.MoviePerson || model.Category == ECategory.All)
                ViewModel.Persons=await moviePersonService.GetListBySearchQuery(model.searchQuery);
            
            return Ok(ViewModel);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CineMax.Services;
using CineMax.Models;

namespace CineMax.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIMovieSearch : ControllerBase
    {
        private readonly ICineService _cineService;
        private readonly ILogger<APIMovieSearch> _logger;
        private readonly IConfiguration _configuration;

        public APIMovieSearch(ICineService cineService, ILogger<APIMovieSearch> logger, IConfiguration configuration)
        {
            _cineService = cineService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("search/movie")]
        public async Task<ActionResult<MovieRepo>> SearchMovies([FromQuery]string query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Please provide a query and an API key");
            }

            try
            {
                var movies = await _cineService.SearchMovies(query);

                if(movies == null || !movies.Any()) 
                {
                    return NotFound("No movies found for the given query");
                }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movies: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get movies");
            }
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<MovieRepo>> GetMovieById([FromRoute] int movieId)
        {
            var apiKey = _configuration["CineMax2024"];
            if (movieId == 0 || string.IsNullOrWhiteSpace(apiKey))
            {
                return BadRequest("Please provide a movie id and an API key");
            }

            try
            {
                var movie = await _cineService.GetMovieById(movieId);

                if (movie == null)
                {
                    return NotFound("No movie found for the given id");
                }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movie: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get movie");
            }
        }

        [HttpGet("movie/{movieId}/credits")]
        public async Task<ActionResult<MovieCreditsRepo>> GetMovieCredits([FromRoute] int movieId)
        {
            var apiKey = _configuration["CineMax2024"];
            if (movieId == 0 || string.IsNullOrWhiteSpace(apiKey))
            {
                return BadRequest("Please provide a movie id and an API key");
            }

            try
            {
                var movieCredits = await _cineService.GetMovieCredits(movieId);

                if (movieCredits == null)
                {
                    return NotFound("No movie credits found for the given id");
                }
                return Ok(movieCredits);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movie credits: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get movie credits");
            }
        }



    }
}

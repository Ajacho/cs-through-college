using CineMax.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CineMax.Services
{
    public class CineService : ICineService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<CineService> _logger;
        private readonly string _apiKey;

        public CineService(HttpClient httpClient, ILogger<CineService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["CineMax2024"];
        }

        public async Task<IEnumerable<MovieRepo>> SearchMovies(string query)
        {
            // Api call to search for movies
            string endPoint = $"https://api.themoviedb.org/3/search/movie?" +
                $"&api_key={_apiKey}" +
                $"&query={Uri.EscapeDataString(query)}" +
                $"&language=en-US&page=1&include_adult=false";

            var response = await _httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var searchResults = JsonSerializer.Deserialize<SearchResults>(responseBody, options);

                return searchResults.Results;

            }
            else
            {
                _logger.LogError($"Failed to get movie :/ -> {response.StatusCode}\n{response.Content}");
                return null;
            }
        }

        public async Task<MovieRepo> GetMovieById(int movieId)
        {
            var endPoint = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={_apiKey}";

            var response = await _httpClient.GetAsync(endPoint);
            if (response.IsSuccessStatusCode) {
                var responseBody = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var movie = JsonSerializer.Deserialize<MovieRepo>(responseBody, options);
                return movie;
            }
            else
            {
                _logger.LogError($"Failed to get movie by id :/ -> {response.StatusCode}\n{response.Content}");
                return null;
            }
        }

        public async Task<MovieCreditsRepo> GetMovieCredits(int movieId)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var movie = JsonSerializer.Deserialize<MovieCreditsRepo>(responseBody, options);
                return movie;
            }
            else
            {
                _logger.LogError($"Failed to get movie :/ -> {response.StatusCode}\n{response.Content}");
                return null;
            }
        }

        public string FormatRuntime(int runtime)
        {
            if (runtime == 0)
            {
                return "not available";
            }
            else if (runtime < 60)
            {
                return $"{runtime}m";
            }

            int hours = runtime / 60;
            int minutes = runtime % 60;

            return $"{hours}h {minutes}m";
        }

        public string FormatReleaseDate(string releaseDate)
        {
            DateTime date = DateTime.Parse(releaseDate);
            return date.ToString("MMMM dd, yyyy");
        }



        public class SearchResults
        {
            [JsonPropertyName("results")]
            public List<MovieRepo> Results { get; set; }
        }

        public class MovieIdResults
        {
            [JsonPropertyName("results")]
            public List<MovieRepo> Results { get; set; }
        }

        public class GetMoviCreditsResults
        {
            [JsonPropertyName("results")]
            public List<MovieCreditsRepo> Results { get; set; }
        }
    }
}
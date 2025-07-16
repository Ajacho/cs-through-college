using CineMax.Models;

namespace CineMax.Services
{
    public interface ICineService
    {
        Task<IEnumerable<MovieRepo>> SearchMovies(string query);
        Task<MovieRepo> GetMovieById(int movieId);
        Task<MovieCreditsRepo> GetMovieCredits(int movieId);

        string FormatRuntime(int runtime);
        string FormatReleaseDate(string releaseDate);

    }
}
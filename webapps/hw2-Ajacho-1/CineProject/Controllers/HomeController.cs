using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CineProject.Models;
using CineProject.DAL.Abstract;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;

namespace CineProject.Controllers;

public class HomeController : Controller
{
    // Dependency injection
    private readonly ILogger<HomeController> _logger;
    private readonly IShowRepository _showRepository;      // Repository instance
    public HomeController(ILogger<HomeController> logger, IShowRepository showRepository)
    {
        _logger = logger;
        _showRepository = showRepository;       // Repository instance
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Info()
    {
        var totalShows = _showRepository.GetAll().Count();
        var totalMovies = _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier == "Movie").Count();
        var totalTVShows = _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier != "Movie").Count();

        var maxTMDBPopularity = _showRepository.GetAll().Max(s => (double?)s.TmdbPopularity) ?? 0.0;
        var maxTMDBShow = _showRepository.GetAll(s => s.TmdbPopularity == maxTMDBPopularity).FirstOrDefault()?.Title ?? "No Show Found";

        var maxIMDBVote = _showRepository.GetAll().Max(s => (int?)s.ImdbVotes) ?? 0;
        var maxIMDBVoteShow = _showRepository.GetAll().OrderByDescending(s => s.ImdbVotes).FirstOrDefault()?.Title ?? "No Show Found";

        // var totalShows = _showRepository.NumberOfShowsByType();
        // var totalMovies = totalShows.movie;
        // var totalTVShows = totalShows.tv;

        // var maxTMDBShow = _showRepository.ShowWithHighestTMDBPopularity()?.Title ?? "No Show Found";
        // var maxTMDBPopularity = _showRepository.ShowWithHighestTMDBPopularity()?.TmdbPopularity ?? 0.0;

        // var maxIMDBVoteShow = _showRepository.ShowWithMostIMDBVotes()?.Title ?? "No Show Found";
        // var maxIMDBVote = _showRepository.ShowWithMostIMDBVotes()?.ImdbVotes ?? 0;
        
        
        var totalGenres = _showRepository.GetAll().SelectMany(s => s.GenreAssignments).Select(sg => sg.Genre).Distinct().Count();
        var genreList = _showRepository.GetAll().SelectMany(s => s.GenreAssignments).Select(sg => sg.Genre).Distinct().OrderBy(s => s.GenreString).ToList();

        var directorShows = _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier != "Movie")
        .SelectMany(s => s.Credits)
        .Where(c => c.Role.RoleName == "Director")
        .Select(c => c.Person)
        .Distinct().Count();
        
        var directorName = _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier != "Movie")
            .SelectMany(s => s.Credits)
            .Where(c => c.Role.RoleName == "Director")
            .Select(c => c.Person).Distinct()
            .FirstOrDefault()?.FullName ?? "No Director Found";
        
        var directorShowTitles= _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier != "Movie")
            .SelectMany(s => s.Credits)
            .Where(c => c.Role.RoleName == "Director")
            .Select(c => c.Show)
            .Distinct()
            .OrderBy(g => g.ReleaseYear)
            .ThenBy(s => s.Title)
            .ToList();

        var directorShowYears= _showRepository.GetAll(s => s.ShowType.ShowTypeIdentifier != "Movie")
            .SelectMany(s => s.Credits)
            .Where(c => c.Role.RoleName == "Director")
            .Select(c => c.Show)
            .Distinct()
            .OrderBy(g => g.ReleaseYear)
            .ToList();
        
        var viewModel = (numShows: totalShows, nuwMovies: totalMovies, numTvShows: totalTVShows, 
        maxPopularity: maxTMDBPopularity, maxTMDBShowName: maxTMDBShow, maxIMDBVotes: maxIMDBVote, 
        maxIMDBShowName: maxIMDBVoteShow, numGenres: totalGenres, genres: genreList, 
        numDirectors: directorShows, directorShowName: directorName, shows: directorShowTitles, years: directorShowYears);

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

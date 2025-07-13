using System.Linq;
using Microsoft.EntityFrameworkCore;
using CineProject.DAL.Abstract;
using CineProject.Models;

// And the associated implementation, stubbed out for you.
// Put this in folder DAL/Concrete

namespace CineProject.DAL.Concrete;

public class ShowRepository : Repository<Show>, IShowRepository
{
    private DbSet<Show> _shows;

    public ShowRepository(StreamingDBDbContext context) : base(context)
    {
        _shows = context.Shows;
    }

    public (int show, int movie, int tv) NumberOfShowsByType()
    {
        // Use _shows to get what you need.  We purposefully don't have access to other dbSets.

        var showCount = _shows.Count();
        var movieCount = _shows.Count(s => s.ShowType.ShowTypeIdentifier == "Movie");
        var tvCount = showCount - movieCount; 
        return (showCount, movieCount, tvCount);

        // return (0, 0, 0);
    }

    public Show ShowWithHighestTMDBPopularity()
    {
        var maxPopularity = _shows.Max(s => (double?)s.TmdbPopularity) ?? 0.0;
        return _shows.FirstOrDefault(s => s.TmdbPopularity == maxPopularity) ?? new Show();
        // return null;
    }

    public Show ShowWithMostIMDBVotes()
    {
        var maxVotes = _shows.Max(s => (int?)s.ImdbVotes) ?? 0;
        return _shows.OrderByDescending(s => s.ImdbVotes).FirstOrDefault() ?? new Show();
        // return null;
    }

}
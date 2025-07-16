using CineMax.Models;
using CineMax.Services;
using Moq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;


namespace CineMax_Tests;

public class Tests
{
    private Mock<CineService> _cineServiceMock;

    [SetUp]
    public void Setup()
    {
        // skipping the HttpClient and Logger for these tests
        _cineServiceMock = new Mock<CineService>(null, null);
        //var httpClientMock = new Mock<HttpClient>();
        //var loggerMock = new Mock<ILogger<CineService>>();

        //_cineServiceMock = new Mock<CineService>(httpClientMock.Object, loggerMock.Object);

    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void TestProjectReference()
    {
        // Arrange
        var movie = new MovieRepo
        {
            Title = "The Shawshank Redemption",
            Runtime = 142,
            Popularity = 9.3,
            Overview = "Two imprisoned"
        };

        Assert.That(movie.Title, Is.EqualTo("The Shawshank Redemption"));
    }

    //Transforming runtime in minutes to a string in hours and monites
    [Test]
    public void CineMax_TestFormatRuntime()
    {
        // Test 200 minutes -> "3 hours 20 minutes"
        string result = _cineServiceMock.Object.FormatRuntime(200);
        Assert.That(result, Is.EqualTo("3h 20m"));

        string result2 = _cineServiceMock.Object.FormatRuntime(45);
        Assert.That(result2, Is.EqualTo("45m"));

        string result3 = _cineServiceMock.Object.FormatRuntime(0);
        Assert.That(result3, Is.EqualTo("not available"));

    }

    //Transforming release date into Month Day, Year format
    [Test]
    public void CineMax_TestFormatReleaseDate()
    {
        // Test "1994-09-10" -> "September 10, 1994"
        string result = _cineServiceMock.Object.FormatReleaseDate("1994-09-10");
        Assert.That(result, Is.EqualTo("September 10, 1994"));
    }
}

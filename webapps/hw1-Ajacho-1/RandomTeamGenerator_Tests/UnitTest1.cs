using NUnit.Framework;
using RandomTeamGenerator.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace RandomTeamGenerator_Tests;

public class TeamGeneratorTests
{
    [SetUp]
    public void SetUp(){
    }

    [Test]
    public void TeamFormation_ShouldInitializeWithGivenValues()
    {
        var teamFormation = new TeamFormation
        {
            Names = "Teagan\nRicky\nHarley",
            TeamSize = 2
        };

        Assert.AreEqual(2, teamFormation.TeamSize, "TeamSize should be set correctly.");
        Assert.AreEqual(0, teamFormation.GroupedTeams.Count, "GroupedTeams should start empty.");
    }

    [Test]
    public void TeamFormation_InvalidTeamSize_ShouldBeInvalid()
    {
        var teamFormation = new TeamFormation {
            Names = "Hayley\nLanden\nAlexis\nMalakai\nTeagan\nRicky\nHarley\nCasey\nZara\nEvan",
            TeamSize = 1
        };
        var validationResults = CheckModelValidation(teamFormation);
        Assert.IsNotEmpty(validationResults, "model should be invalid");
    }

    [Test]
    public void TeamFormation_InvalidNames_ShouldBeInvalid()
    {
        var teamFormation = new TeamFormation
        {
            Names = "Alexis\nBob@Charlie", 
            TeamSize = 3
        };

        var validationResults = CheckModelValidation(teamFormation);
        Assert.IsNotEmpty(validationResults, "disallowed characters in names");
    }

    [Test]
    public void GroupedTeams_ShouldCreateCorrectNumberOfGroups()
    {
        var teamFormation = new TeamFormation
        {
            Names = "Hayley\nLanden\nAlexis\nMalakai\nTeagan\nRicky", 
            TeamSize = 3 
        };

        var namesList = teamFormation.Names.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var groupedTeams = new List<List<string>>();

        for (int i = 0; i < namesList.Count; i += teamFormation.TeamSize)
        {
            groupedTeams.Add(namesList.Skip(i).Take(teamFormation.TeamSize).ToList());
        }
        Assert.AreEqual(2, groupedTeams.Count); 
        Assert.AreEqual(3, groupedTeams[0].Count); 
    }
    private IList<ValidationResult> CheckModelValidation(TeamFormation teamFormationInstance)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(teamFormationInstance);
        Validator.TryValidateObject(teamFormationInstance, context, results, true);
        return results;
    }


}
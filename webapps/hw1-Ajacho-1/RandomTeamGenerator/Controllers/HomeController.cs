using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RandomTeamGenerator.Models;

namespace RandomTeamGenerator.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public ViewResult TeamForm(){
        return View(new TeamFormation());
    }   

    [HttpPost]
    public ActionResult TeamForm(TeamFormation teamFormation){

        if (teamFormation.Names != null && teamFormation.TeamSize > 0){
            var namesList = teamFormation.Names.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(name => name.Trim())
                                                .ToList();
    
            var shuffledNames = namesList.OrderBy(x => Guid.NewGuid()).ToList();

            for(int i = 0; i < shuffledNames.Count; i += teamFormation.TeamSize){
                var group = shuffledNames.Skip(i).Take(teamFormation.TeamSize).ToList();
                teamFormation.GroupedTeams.Add(group);
            }
        }   
        return View("TeamForm", teamFormation);
    }
}

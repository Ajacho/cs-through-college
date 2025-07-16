using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

namespace RandomTeamGenerator.Models{
    public class TeamFormation {
        [Required(ErrorMessage = "Names are required.")]
        [RegularExpression(@"^[a-zA-Z\s,.\-_'']+$", ErrorMessage = "Names can only contain letters, spaces, and the characters ,.-_'")]
        public string Names {get; set;}

        [Required(ErrorMessage = "Team size is required")]
        [Range(2, 10, ErrorMessage ="Team size must only be between 2 and 10")]
        public int TeamSize {get; set;}
        public List<List<string>> GroupedTeams {get; set;} = new List<List<string>>();
    }
}



using System.ComponentModel.DataAnnotations;

namespace EloService.Models
{
    public class MatchResult
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Completed { get; set; }

        public bool DidTeam1Win { get; set; }

        public string Team1Members { get; set; } = null!;

        public string Team2Members { get; set; } = null!;
    }
}

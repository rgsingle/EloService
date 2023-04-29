namespace EloService.Dtos
{
    public class MatchResultDto
    {
        public bool DidTeam1Win { get; set; }

        public IEnumerable<int> Team1 { get; set; } = null!;

        public IEnumerable<int> Team2 { get; set; } = null!;
    }
}
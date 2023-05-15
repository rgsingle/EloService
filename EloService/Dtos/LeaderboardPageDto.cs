namespace EloService.Dtos
{
    public class LeaderboardPageDto
    {
        public int Page { get; set; }

        public int NumPages { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; } = null!;

        public int PageSize { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace EloService.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected Context()
        {
        }

        public async Task<IEnumerable<Player>> GetOrCreatePlayersAsync(IEnumerable<int> playerIds)
        {
            var players = new List<Player>();

            foreach (var playerId in playerIds)
            {
                var player = await Players
                    .SingleOrDefaultAsync(p => p.UserId == playerId);

                if (player == null)
                {
                    player = new Player()
                    {
                        UserId = playerId,
                        Elo = 1000
                    };

                    await Players.AddAsync(player);
                }

                players.Add(player);
            }

            return players;
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<MatchResult> MatchResults { get; set; }
    }
}

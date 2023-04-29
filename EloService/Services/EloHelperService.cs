using EloService.Models;

namespace EloService.Services
{
    public class EloHelperService
    {
        private const int MaxEloGain = 50;
        private const int EloK = 32;
        private const int MinEloGain = 14;

        private readonly ILogger<EloHelperService> _logger;

        public EloHelperService(ILogger<EloHelperService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calculates the expectation for player 1 to win
        /// </summary>
        public double ProbabilityToWin(int player1Rating, int player2Rating)
        {
            return 1 / (1 + Math.Pow(10, (player2Rating - player1Rating) / 400.0));
        }

        /// <summary>
        /// Calculates the Delta value for the elos if player 1 were to win.
        /// If player 1 wins, add the delta to player 1 and subtract from player 2.
        /// If player 2 wins, add the delta to player 2 and subtract from player 1.
        /// </summary>
        public int CalculateEloDelta(double winProbability)
        {
            int delta = (int)(EloK * (1 - winProbability));

            delta = Math.Max(delta, MinEloGain);
            delta = Math.Min(delta, MaxEloGain);

            return delta;
        }

        /// <summary>
        /// Sum the players's elos
        /// </summary>
        public int CalculateTeamElo(IEnumerable<Player> players)
        {
            return players.Sum(p => p.Elo);
        }

        /// <summary>
        /// Update the elo for two teams of players
        /// </summary>
        public void UpdateElos(IEnumerable<Player> team1, IEnumerable<Player> team2, bool team1Win)
        {
            var team1Elo = CalculateTeamElo(team1);
            var team2Elo = CalculateTeamElo(team2);

            if (team1Win)
            {
                var probability = ProbabilityToWin(team1Elo, team2Elo);
                var delta = CalculateEloDelta(probability);

                foreach(Player p in team1)
                    p.Elo += delta;
                foreach (Player p in team2)
                    p.Elo -= delta;

                _logger.LogDebug("Updated elos for: Winners = [{0}], Losers = [{1}]. Probability = {2}%, Gain = {3} points.",
                    string.Join(", ", team1.Select(p => p.UserId)), string.Join(", ", team2.Select(p => p.UserId)), probability * 100, delta);
            }
            else
            {
                var probability = ProbabilityToWin(team2Elo, team1Elo);
                var delta = CalculateEloDelta(probability);

                foreach (Player p in team2)
                    p.Elo += delta;
                foreach (Player p in team1)
                    p.Elo -= delta;

                _logger.LogDebug("Updated elos for: Winners = [{0}], Losers = [{1}]. Probability = {2}%, Gain = {3} points.",
                    string.Join(", ", team2.Select(p => p.UserId)), string.Join(", ", team1.Select(p => p.UserId)), probability * 100, delta);
            }
        }

        /// <summary>
        /// Update the win/loss/streak values for each player
        /// </summary>
        public void UpdateWinLoss(IEnumerable<Player> team, bool didTeamWin)
        {
            foreach (var player in team)
            {
                if (didTeamWin)
                {
                    player.CurrentWinstreak++;

                    if (player.CurrentWinstreak > player.LongestWinstreak)
                        player.LongestWinstreak = player.CurrentWinstreak;

                    player.CurrentLossstreak = 0;
                    player.Wins++;
                }
                else
                {
                    player.CurrentLossstreak++;

                    if (player.CurrentLossstreak > player.LongestLossstreak)
                        player.LongestLossstreak = player.CurrentLossstreak;

                    player.CurrentWinstreak = 0;
                    player.Losses++;
                }

                player.WinLossRatio = (double)player.Wins / (player.Wins + player.Losses);
            }
        }
    }
}

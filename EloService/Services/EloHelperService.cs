using EloService.Models;

namespace EloService.Services
{
    public class EloHelperService
    {
        private readonly ILogger<EloHelperService> _logger;

        public EloHelperService(ILogger<EloHelperService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calculates the expectation for team 1 to win
        /// </summary>
        private static double ProbabilityToWin(IEnumerable<Player> team1, IEnumerable<Player> team2)
        {
            var team1Elo = CalculateTeamElo(team1);
            var team2Elo = CalculateTeamElo(team2);

            return 1 / (1 + Math.Pow(10, (team1Elo - team2Elo) / 400.0));
        }

        /// <summary>
        /// Calculate the K-Factor (Elo gain/loss) for a player
        /// </summary>
        private static double CalculateKFactor(Player player)
        {
            // US Chess Foundation (USCF)
            if (player.Elo < 2100)
                return 32;
            else if (player.Elo < 2400)
                return 24;
            else
                return 16;
        }

        /// <summary>
        /// Calculates the Delta value for the elos if player 1 were to win.
        /// If player 1 wins, add the delta to player 1 and subtract from player 2.
        /// If player 2 wins, add the delta to player 2 and subtract from player 1.
        /// </summary>
        private static int CalculateEloDelta(Player player, double winProbability)
        {
            int delta = (int)(CalculateKFactor(player) * (1 - winProbability));

            // TODO: Bound this?

            return delta;
        }

        /// <summary>
        /// Sum the players's elos
        /// </summary>
        private static int CalculateTeamElo(IEnumerable<Player> players)
        {
            return players.Sum(p => p.Elo);
        }

        /// <summary>
        /// Update the win/loss/streak values for each player
        /// </summary>
        private static void UpdateWinLoss(IEnumerable<Player> team, bool didTeamWin)
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
            }
        }

        /// <summary>
        /// Update the elo for two teams of players
        /// </summary>
        public void UpdateElos(IEnumerable<Player> team1, IEnumerable<Player> team2, bool team1Win)
        {
            IEnumerable<Player> winners = team1Win ? team1 : team2;
            IEnumerable<Player> losers = team1Win ? team2 : team1;
            double probabilityToWin = ProbabilityToWin(winners, losers);

            // Update Elos
            foreach (Player player in winners)
                player.Elo += CalculateEloDelta(player, probabilityToWin);

            foreach (Player player in losers)
                player.Elo -= CalculateEloDelta(player, probabilityToWin);

            // Log Results
            _logger.LogDebug("Updated elos for: Winners = [{winners}], Losers = [{losers}]. Probability = {prob}%.",
                string.Join(", ", winners.Select(p => p.UserId)), string.Join(", ", losers.Select(p => p.UserId)), probabilityToWin * 100);

            // Update Highest Elos
            foreach (Player player in team1)
                player.HighestElo = Math.Max(player.HighestElo, player.Elo);

            foreach (Player player in team2)
                player.HighestElo = Math.Max(player.HighestElo, player.Elo);

            // Update Win/Loss Record
            UpdateWinLoss(winners, true);
            UpdateWinLoss(losers, false);
        }
    }
}

using EloService.Dtos;
using EloService.Models;
using EloService.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EloService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchResultsController : ControllerBase
    {
        private readonly Context _context;
        private readonly EloHelperService _eloHelper;

        public MatchResultsController(Context context, EloHelperService eloHelper)
        {
            _context = context;
            _eloHelper = eloHelper;
        }

        // POST api/<MatchResultsController>
        [HttpPost]
        public async void Post([FromBody] MatchResultDto value)
        {
            var match = new MatchResult()
            {
                Completed = DateTime.UtcNow,
                DidTeam1Win = value.DidTeam1Win,
                Team1Members = string.Join(',', value.Team1),
                Team2Members = string.Join(',', value.Team2)
            };

            _context.MatchResults.Add(match);

            // Load or create Team Members
            var team1 = await _context.GetOrCreatePlayersAsync(value.Team1);
            var team2 = await _context.GetOrCreatePlayersAsync(value.Team2);

            // Update Elos
            _eloHelper.UpdateElos(team1, team2, value.DidTeam1Win);

            // Update WinLoss
            _eloHelper.UpdateWinLoss(team1, value.DidTeam1Win);
            _eloHelper.UpdateWinLoss(team2, !value.DidTeam1Win);

            await _context.SaveChangesAsync();
        }
    }
}

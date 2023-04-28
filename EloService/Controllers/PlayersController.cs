using AutoMapper;
using EloService.Dtos;
using EloService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EloService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public PlayersController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<PlayersController>
        [HttpGet]
        public IEnumerable<PlayerDto> Get()
        {
            return _context.Players
                .ToArray()
                .Select(p => _mapper.Map<PlayerDto>(p));
        }

        // GET api/<PlayersController>/5
        [HttpGet("{id}")]
        public ActionResult<PlayerDto> Get(int id)
        {
            var player = _context.Players
                .FirstOrDefault(p => p.UserId == id);

            if (player == null)
                return NotFound();

            return _mapper.Map<PlayerDto>(player);
        }

        // POST api/<PlayersController>
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> PostAsync([FromBody] PlayerDto value)
        {
            var player = _context.Players
                .FirstOrDefault(p => p.UserId == value.UserId);

            if (player == null)
            {
                player = new Player()
                {
                    UserId = value.UserId,
                    CurrentLossstreak = value.CurrentLossstreak,
                    CurrentWinstreak = value.CurrentWinstreak,
                    Elo = value.Elo,
                    LongestLossstreak = value.LongestLossstreak,
                    LongestWinstreak = value.LongestWinstreak,
                    Losses = value.Losses,
                    WinLossRatio = value.WinLossRatio,
                    Wins = value.Wins,
                    XpMultiplier = value.XpMultiplier
                };

                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Post", _mapper.Map<PlayerDto>(player));
            }
            else
            {
                player.Wins = value.Wins;
                player.Losses = value.Losses;

                // TODO: Win/Loss Calculation, ignore winlossratio in Dto
                player.WinLossRatio = value.WinLossRatio;

                player.CurrentWinstreak = value.CurrentWinstreak;
                player.CurrentLossstreak = value.CurrentLossstreak;
                player.LongestLossstreak = value.LongestLossstreak;
                player.LongestWinstreak = value.LongestWinstreak;

                // TODO: Elo Calculation, ignore winlossratio in Dto
                player.Elo = value.Elo;

                player.XpMultiplier = value.XpMultiplier;
                
                _context.Players.Update(player);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<PlayerDto>(player));
            }
        }

        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var player = _context.Players
                .FirstOrDefault(p => p.UserId == id);

            if(player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

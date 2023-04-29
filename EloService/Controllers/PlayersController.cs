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

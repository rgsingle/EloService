using AutoMapper;
using EloService.Dtos;
using EloService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EloService.Controllers
{
    [Route("api/Leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public LeaderboardController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<LeaderboardPageDto> GetLeaderboard(int pageSize = 10, int page = 0)
        {
            var ret = new LeaderboardPageDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = await _context.Players.CountAsync(),
                Players = (await _context.Players
                    .OrderByDescending(p => p.Elo)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync())
                    .Select(p => _mapper.Map<PlayerDto>(p))
            };

            ret.NumPages = (int)Math.Ceiling((double)ret.TotalCount / pageSize);

            return ret;
        }

        [HttpGet("{id}")]
        public async Task<int> GetLeaderboardPosition(int playerId)
        {
            return await _context.Players
                .OrderByDescending(p => p.Elo)
                .TakeWhile(p => p.UserId != playerId)
                .CountAsync();
        }
    }
}

using AutoMapper;
using EloService.Dtos;
using EloService.Models;

namespace EloService
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<MatchResult, MatchResultDto>();
        }
    }
}

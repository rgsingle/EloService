using AutoMapper;
using EloForDumDums.Dtos;
using EloForDumDums.Models;

namespace EloForDumDums
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Player, PlayerDto>();
        }
    }
}

using AutoMapper;
using Domain.Entities;
using Services.Queries.DTOs.DonutDTOs;

namespace Services.Mappers
{
    public class DonutProfile : Profile
    {
        public DonutProfile() 
        {
            CreateMap<Donut, DonutDTO>();
        }
    }
}

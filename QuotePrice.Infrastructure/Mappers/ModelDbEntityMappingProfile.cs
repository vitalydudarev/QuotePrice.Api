using AutoMapper;
using QuotePrice.Domain.Models;
using QuotePrice.Infrastructure.Database.Entities;

namespace QuotePrice.Infrastructure.Mappers;

public class ModelDbEntityMappingProfile : Profile
{
    public ModelDbEntityMappingProfile()
    {
        CreateMap<Quote, DbQuote>().ReverseMap();
    }
}
using AutoMapper;
using QuotePrice.Api.DTOs;
using QuotePrice.Domain.Models;

namespace QuotePrice.Api.Mappers;

public class ModelDtoMappingProfile : Profile
{
    public ModelDtoMappingProfile()
    {
        CreateMap<Quote, QuoteDto>()
            .ForMember(a => a.Timestamp, expr => expr.MapFrom(b => RoundDouble(b.Timestamp)));
    }

    // this conversion may affect the performance
    private static double? RoundDouble(double? value)
    {
        return value.HasValue ? (double)Math.Round((decimal)value, 2) : null;
    }
}
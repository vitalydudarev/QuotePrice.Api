using System.Globalization;
using AutoMapper;
using QuotePrice.Domain.Models;
using QuotePrice.Infrastructure.Responses;

namespace QuotePrice.Infrastructure.Mappers;

public class ResponseModelMappingProfile : Profile
{
    public ResponseModelMappingProfile()
    {
        CreateMap<BitfinexQuoteResponse, Quote>()
            .ForMember(a => a.Ask, expr => expr.MapFrom(b => ConvertDouble(b.Ask)))
            .ForMember(a => a.Bid, expr => expr.MapFrom(b => ConvertDouble(b.Bid)))
            .ForMember(a => a.High, expr => expr.MapFrom(b => ConvertDouble(b.High)))
            .ForMember(a => a.Last, expr => expr.MapFrom(b => ConvertDouble(b.LastPrice)))
            .ForMember(a => a.Low, expr => expr.MapFrom(b => ConvertDouble(b.Low)))
            .ForMember(a => a.Timestamp, expr => expr.MapFrom(b => ConvertDouble(b.Timestamp)))
            .ForMember(a => a.Volume, expr => expr.MapFrom(b => ConvertDouble(b.Volume)));
        
        CreateMap<BitstampQuoteResponse, Quote>()
            .ForMember(a => a.Ask, expr => expr.MapFrom(b => ConvertDouble(b.Ask)))
            .ForMember(a => a.Bid, expr => expr.MapFrom(b => ConvertDouble(b.Bid)))
            .ForMember(a => a.High, expr => expr.MapFrom(b => ConvertDouble(b.High)))
            .ForMember(a => a.Last, expr => expr.MapFrom(b => ConvertDouble(b.Last)))
            .ForMember(a => a.Low, expr => expr.MapFrom(b => ConvertDouble(b.Low)))
            .ForMember(a => a.Timestamp, expr => expr.MapFrom(b => ConvertDouble(b.Timestamp)))
            .ForMember(a => a.Volume, expr => expr.MapFrom(b => ConvertDouble(b.Volume)));
    }
    
    private static double? ConvertDouble(string? s)
    {
        return !string.IsNullOrEmpty(s) ? double.Parse(s, CultureInfo.InvariantCulture) : null;
    }
}
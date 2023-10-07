using AutoMapper;
using CA.Application.DTOs.Ent.Selection;
using CA.Application.DTOs.Ent.TValue;
using CA.Domain.Ent;

namespace CA.Application.Profiles
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Selection
            CreateMap<Selection, SelectionDto>().ReverseMap();
            CreateMap<Selection, SelectionCreateDto>().ReverseMap();
            CreateMap<Selection, SelectionUpdateDto>().ReverseMap();
            #endregion

            #region TValue
            CreateMap<TValue, TValueDto>().ReverseMap();
            #endregion
        }
    }
}

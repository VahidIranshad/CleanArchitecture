using AutoMapper;
using CA.Application.DTOs.Ent.Selection;
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
        }
    }
}

using AutoMapper;
using CA.Application.DTOs.Ent.Selection;
using CA.Application.DTOs.Ent.TValue;
using CA.Application.Profiles;
using CA.Domain.Ent;
using System.Runtime.Serialization;

namespace UnitTest.Mapping
{
    public class MappingTests
    {

        private static IConfigurationProvider _configuration;
        private static IConfigurationProvider configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                    });
                }
                return _configuration;
            }
        }
        private static IMapper _mapper;
        private static IMapper mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = configuration.CreateMapper();
                }
                return _mapper;
            }

        }
        [Theory]
        [InlineData(typeof(SelectionDto), typeof(Selection))]
        [InlineData(typeof(Selection), typeof(SelectionDto))]
        [InlineData(typeof(SelectionCreateDto), typeof(Selection))]
        [InlineData(typeof(SelectionUpdateDto), typeof(Selection))]
        [InlineData(typeof(TValue), typeof(TValueDto))]
        [InlineData(typeof(TValueDto), typeof(TValue))]
        public void Map_SourceToDestination_ExistConfiguration(System.Type origin, System.Type destination)
        {
            var instance = FormatterServices.GetUninitializedObject(origin);

            mapper.Map(instance, origin, destination);
        }
    }
}

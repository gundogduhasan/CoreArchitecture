using CoreArchitecture.Common.DTOs;
using CoreArchitecture.Data.Entities;
using AutoMapper;

namespace CoreArchitecture.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TestDepartment, TestDepartmentDto>().ReverseMap();

            CreateMap<TestPerson, TestPersonDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department!.Name))
                .ForMember(dest => dest.PersonDepartment, opt => opt.MapFrom(src => src.Department))
                .ReverseMap();
        }
    }

}

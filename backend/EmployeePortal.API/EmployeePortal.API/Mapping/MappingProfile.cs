using AutoMapper;
using EmployeePortal.API.Data.Models;
using EmployeePortal.API.DTOs;

namespace EmployeePortal.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}

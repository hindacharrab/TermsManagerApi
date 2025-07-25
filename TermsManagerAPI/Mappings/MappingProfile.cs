using AutoMapper;
using CGUManagementAPI.Dtos;
using CGUManagementAPI.Models;
using TermsManagerAPI.Dtos;

namespace TermsManagerAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            // Mapping User → UserWithCGUStatusDto
            CreateMap<User, UserWithCGUStatusDto>();


            // Mapping CreateUserRequest → User
            CreateMap<User, CreateUserRequest>().ReverseMap();

            //Mapping UpdateProfileRequest/Dtos

            CreateMap<User, UpdateProfileRequest>().ReverseMap();
            CreateMap<User, UserProfileDto>().ReverseMap();

            
        }


    }
}

using AutoMapper;
using Auth.Lextatico.Application.Dtos.User;
using Auth.Lextatico.Domain.Models;

namespace Auth.Lextatico.Application.AutoMapper.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // DTO TO MODEL

            // MODEL TO DTO
            CreateMap<ApplicationUser, UserDetailDto>();
        }
    }
}

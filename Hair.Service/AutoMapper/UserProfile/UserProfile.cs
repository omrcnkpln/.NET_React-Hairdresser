using AutoMapper;
using Hair.Core.Dtos.AddDtos;
using Hair.Core.Dtos.ResponseDtos;
using Hair.Core.Dtos.UpdateDtos;
using Hair.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hair.Service.AutoMapper.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}

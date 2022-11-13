using System;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer.DataTransferObjects;

namespace CIT_2022_Portfolio2.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserPageDTO, UserModel>().ReverseMap();
        }
    }
}


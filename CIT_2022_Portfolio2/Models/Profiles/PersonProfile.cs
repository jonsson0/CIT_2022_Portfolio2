﻿using System;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer.DataTransferObjects;

namespace CIT_2022_Portfolio2.Models.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonOnMainPageDTO, PersonModel>().ReverseMap();
        }
    }
}


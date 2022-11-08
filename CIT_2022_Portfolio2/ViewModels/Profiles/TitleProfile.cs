﻿using AutoMapper;
using DataLayer.DataTransferObjects;
using DataLayer.Models;


namespace CIT_2022_Portfolio2.ViewModels.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleOnMainPage, TitleViewModel>();

            CreateMap<TitleViewModel, TitleOnMainPage>();
        }
    }
}
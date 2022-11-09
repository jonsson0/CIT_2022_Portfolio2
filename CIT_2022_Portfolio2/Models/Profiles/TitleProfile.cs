using AutoMapper;
using CIT_2022_Portfolio2.models;
using DataLayer.DataTransferObjects;
using DataLayer.Models;


namespace CIT_2022_Portfolio2.ViewModels.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleOnMainPageDTO, TitleModel>();

            CreateMap<TitleModel, TitleOnMainPageDTO>();
        }
    }
}

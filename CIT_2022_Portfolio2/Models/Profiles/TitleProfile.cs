using AutoMapper;
using CIT_2022_Portfolio2.models;
using CIT_2022_Portfolio2.Models;
using DataLayer.DataTransferObjects;
using DataLayer.Models;


namespace CIT_2022_Portfolio2.models.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleOnMainPageDTO, TitleModel>().ReverseMap();

            CreateMap<Similar_Title, SimilarTitlesModel>().ReverseMap();
        }
    }
}

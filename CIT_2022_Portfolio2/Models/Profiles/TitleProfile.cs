using AutoMapper;
using DataLayer.DataTransferObjects;
using DataLayer.Models;



namespace CIT_2022_Portfolio2.Models.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleOnMainPageDTO, TitleModel>().ReverseMap();

            CreateMap<Similar_Title, CoActorPersonModel>().ReverseMap();
        }
    }
}

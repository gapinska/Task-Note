using Application.Models.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BoardForCreationDto, Board>().ReverseMap();
            CreateMap<BoardForEditionDto, Board>().ReverseMap();
            CreateMap<BoardForListDto, Board>().ReverseMap();

            CreateMap<LabelForCreationDto, Label>().ReverseMap();
            CreateMap<LabelForEditionDto, Label>().ReverseMap();
            CreateMap<LabelForListDto, Label>().ReverseMap();

            CreateMap<QuestForCreationDto, Quest>().ReverseMap();
            CreateMap<QuestForEditionDto, Quest>().ReverseMap();
            CreateMap<QuestForListDto, Quest>().ReverseMap();

            CreateMap<UserForListDto, User>().ReverseMap();
            CreateMap<UserForLoginDto, User>().ReverseMap();
            CreateMap<UserForCreationDto, User>().ReverseMap();

            CreateMap<MessageDto, MessagePost>().ReverseMap();
            CreateMap<MessageForGetDto, MessagePost>().ReverseMap();
        }
    }
}
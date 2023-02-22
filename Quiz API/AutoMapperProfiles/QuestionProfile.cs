using AutoMapper;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.AutoMapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(QD => QD.Answers, opt => opt.MapFrom(Q => Q.Answers))
                .ForMember(QD => QD.Categorys, opt => opt.MapFrom(Q => Q.Categorys));
        }
    }
}

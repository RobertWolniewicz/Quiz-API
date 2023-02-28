using AutoMapper;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.AutoMapperProfiles
{
    public class QuizQuestionProfile : Profile
    {
        public QuizQuestionProfile()
        {
            CreateMap<Question, QuizQuestion>()
                .ForMember(QQ => QQ.Answers, opt => opt.MapFrom(Q => Q.Answers));
        }
    }
}

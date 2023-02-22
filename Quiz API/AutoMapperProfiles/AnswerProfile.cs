using AutoMapper;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.AutoMapperProfiles
{
    public class AnswerProfile: Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDto>();
        }
    }
}

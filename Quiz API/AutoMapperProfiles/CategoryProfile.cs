using AutoMapper;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}

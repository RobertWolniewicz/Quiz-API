using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface ICategoryServices
    {
        Task<CategoryDto> Create(CategoryDto categoryModel);
        Task Delete(int id);
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(int id);
        Task Update(CategoryDto updatingData);
    }

    public class CategoryServices : ICategoryServices
    {
        private readonly AppDB _dbContext;
        private readonly IMapper _mapper;

        public async Task<List<CategoryDto>> GetAll()
        {
            var categorys = await _dbContext.categories.ToListAsync();
            var categotysDtos = _mapper.Map<List<CategoryDto>>(categorys);
            return categotysDtos;
        }
        public async Task<CategoryDto> GetById(int id)
        {
            var category =await FindById(id);
            var categotysDto = _mapper.Map<CategoryDto>(category);
            return categotysDto;
        }
        public async Task Delete(int id)
        {
            var category = await FindById(id);
            _dbContext.categories.Remove(category);
            _dbContext.SaveChangesAsync();
        }
        public async Task<CategoryDto> Create(CategoryDto categoryModel)
        {
            var newCategory = new Category()
            {
                Name = categoryModel.Name,
            };
            _dbContext.categories.Add(newCategory);
            _dbContext.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(newCategory);
        }
        public async Task Update(CategoryDto updatingData)
        {
            var updatingCategory = await FindById(updatingData.Id);
            updatingCategory.Name = updatingData.Name;
            _dbContext.SaveChangesAsync();
        }
        async Task<Category> FindById(int Id)
        {
           var  result = await _dbContext.categories.FirstOrDefaultAsync(c => c.Id == Id);
           if (result == null)
           {
              throw new NotFoundException("Question not found");
           }
           return result;
        }
    }
}

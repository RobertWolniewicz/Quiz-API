using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Exceptions;
using Quiz_API.Models;
using Sieve.Models;
using Sieve.Services;

namespace Quiz_API.Services
{
    public interface ICategoryServices
    {
        Task<CategoryDto> Create(CategoryDto categoryModel);
        Task Delete(int id);
        Task<PageResult<CategoryDto>> GetAll(SieveModel query, ISieveProcessor sieveProcessor);
        Task<CategoryDto> GetById(int id);
        Task Update(CategoryDto updatingData);
    }
    public class CategoryServices : ICategoryServices
    {
        private readonly AppDB _dbContext;
        private readonly IMapper _mapper;
        public CategoryServices(AppDB DbContext, IMapper mapper)
        {
            _dbContext = DbContext;
            _mapper = mapper;
        }
        public async Task<PageResult<CategoryDto>> GetAll(SieveModel query, ISieveProcessor sieveProcessor)
        {
            var categorysQuery = _dbContext.categories.AsQueryable();
            var categorys =  await sieveProcessor.Apply(query, categorysQuery).ToListAsync();
            var categotysDtos = _mapper.Map<List<CategoryDto>>(categorys);
            var totalCount = await sieveProcessor.Apply(query, categorysQuery, applyPagination: false, applySorting: false).CountAsync();
            return new PageResult<CategoryDto>(categotysDtos, totalCount, query.PageSize.Value, query.Page.Value);
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
            await _dbContext.SaveChangesAsync();
        }
        public async Task<CategoryDto> Create(CategoryDto categoryModel)
        {
            var newCategory = new Category()
            {
                Name = categoryModel.Name,
            };
            _dbContext.categories.Add(newCategory);
           await  _dbContext.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(newCategory);
        }
        public async Task Update(CategoryDto updatingData)
        {
            var updatingCategory = await FindById(updatingData.Id);
            updatingCategory.Name = updatingData.Name;
            await _dbContext.SaveChangesAsync();
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

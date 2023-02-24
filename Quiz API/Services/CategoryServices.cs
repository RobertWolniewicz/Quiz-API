using AutoMapper;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface ICategoryServices
    {
        CategoryDto Create(CategoryDto categoryModel);
        void Delete(int id);
        List<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        void Update(CategoryDto updatingData);
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
        public List<CategoryDto> GetAll()
        {
            var categorys = _dbContext.categories.ToList();
            var categotysDtos = _mapper.Map<List<CategoryDto>>(categorys);
            return categotysDtos;
        }
        public CategoryDto GetById(int id)
        {
            var category = _dbContext.categories.FirstOrDefault(c => c.Id == id);
            var categotysDto = _mapper.Map<CategoryDto>(category);
            return categotysDto;
        }
        public void Delete(int id)
        {
            _dbContext.categories.Remove(_dbContext.categories.FirstOrDefault(c => c.Id == id));
            _dbContext.SaveChanges();
        }
        public CategoryDto Create(CategoryDto categoryModel)
        {
            var newCategory = new Category()
            {
                Name = categoryModel.Name,
            };
            _dbContext.categories.Add(newCategory);
            _dbContext.SaveChanges();
            return _mapper.Map<CategoryDto>(newCategory);
        }
        public void Update(CategoryDto updatingData)
        {
            var updatingCategory = _dbContext.categories.FirstOrDefault(c => c.Id == updatingData.Id);
            updatingCategory.Name = updatingData.Name;
            _dbContext.SaveChanges();
        }
    }
}

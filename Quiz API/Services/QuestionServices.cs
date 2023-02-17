using Quiz_API.Entity;

namespace Quiz_API.Services
{
    public class QuestionServices<T> where T : Question
    {
        private readonly AppDB _dbContext;

        public QuestionServices(AppDB DbContext)
        {
            _dbContext = DbContext;
        }
        public List<T> GetAll() 
        {
            return _dbContext.Set<T>().ToList();
        }
        public Question GetById(int id) 
        {
            return _dbContext.Set<T>().FirstOrDefault(q=>q.Id==id);
        }
        public void Delete(int id) 
        {
            _dbContext.Set<T>().Remove(GetById(id));
            _dbContext.SaveChanges();
        }
        public void Update(T updatingQuestion)
        {
            var existingQuestion = GetById(updatingQuestion.Id);
            if (existingQuestion == null) return;
            existingQuestion = updatingQuestion;
            _dbContext.SaveChanges();
        }
        public void Create(T newQuestion)
        {
            if (newQuestion == null) return;
            _dbContext.Set<T>().Add(newQuestion);
            _dbContext.SaveChanges();
        }
    }
}

using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Services
{
    public interface IQuestionServices
    {
        void Create<T>(DtoQuestion newQuestion) where T : Question, new();
        void Delete<T>(int id) where T : Question;
        List<T> GetAll<T>() where T : Question;
        T GetById<T>(int id) where T : Question;
        void Update<T>(T updatingQuestion) where T : Question;
    }

    public class QuestionServices : IQuestionServices
    {
        private readonly AppDB _dbContext;

        public QuestionServices(AppDB DbContext)
        {
            _dbContext = DbContext;
        }
        public List<T> GetAll<T>() where T : Question => _dbContext.Set<T>().ToList();
        public T GetById<T>(int id) where T : Question => _dbContext.Set<T>().FirstOrDefault(q => q.Id == id);
        public void Delete<T>(int id) where T : Question
        {
            _dbContext.Set<T>().Remove(GetById<T>(id));
            _dbContext.SaveChanges();
        }
        public void Update<T>(T updatingQuestion) where T : Question
        {
            var existingQuestion = GetById<T>(updatingQuestion.Id);
            if (existingQuestion == null) return;
            existingQuestion.QuestionText = updatingQuestion.QuestionText;
            existingQuestion.CorrectAnswer = updatingQuestion.CorrectAnswer;
            existingQuestion.Categorys = updatingQuestion.Categorys;
            existingQuestion.Answers = updatingQuestion.Answers;
            _dbContext.SaveChanges();
        }
        public void Create<T>(DtoQuestion newQuestion) where T : Question, new()
        {
            if (newQuestion == null) return;
            T createdQuestion = new()
            {
                QuestionText = newQuestion.Text,
            };
           foreach(var categoryName in newQuestion.Categorys)
            {
                var Category=_dbContext.categories.FirstOrDefault(c => c.Name == categoryName);
                if (Category == null)
                {
                    Category = new()
                    {
                        Name = categoryName,
                    };
                    _dbContext.categories.Add(Category);
                }
                createdQuestion.Categorys.Add(Category);
            }

            if (createdQuestion is not EasyQuestion)
            {
                createdQuestion.CorrectAnswer = newQuestion.CorrectAnswer;
                _dbContext.Set<T>().Add(createdQuestion);
                foreach (var answer in newQuestion.Answers)
                {
                    var newAnswer = new Answer()
                    {
                        Text = answer
                    };
                    createdQuestion.Answers.Add(newAnswer);
                }
            }
            else
            {
                createdQuestion.CorrectAnswer = (bool)newQuestion.EasyAnswer;
            }
                _dbContext.SaveChanges();
        }
    }
}

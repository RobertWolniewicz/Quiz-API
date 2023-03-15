using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Validators
{
    public class QuestionValidator : AbstractValidator<NewQuestionModel>
    {
        public QuestionValidator(AppDB DbContext)
        {
            RuleFor(c => c.QuestionText).NotEmpty()
                .Custom((value, context) =>
                {
                    var exist = DbContext.Questions.Any(q => q.QuestionText == value);
                    if (exist)
                    {
                        context.AddFailure("QuestionText", "That cquestion exist");
                    }
                });
            RuleFor(c => c.CorrectAnswer).NotEmpty();
            RuleFor(c => c.Categorys).NotEmpty();
        }
    }
}

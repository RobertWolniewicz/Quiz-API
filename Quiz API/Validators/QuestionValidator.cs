using FluentValidation;
using Quiz_API.Models;

namespace Quiz_API.Validators
{
    public class QuestionValidator : AbstractValidator<QuestionDto>
    {
        public QuestionValidator()
        {
            RuleFor(c => c.QuestionText ).NotEmpty();
            RuleFor(c => c.CorrectAnswer).NotEmpty();
            RuleFor(c => c.Categorys).NotEmpty();
        }
    }
}

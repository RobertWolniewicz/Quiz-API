using FluentValidation;
using Quiz_API.Models;

namespace Quiz_API.Validators
{
    public class QuizValidator : AbstractValidator<QuizParameters>
    {
        public QuizValidator()
        {
            RuleFor(qp => qp.Category).NotEmpty();
            RuleFor(qp => qp.NumberOfEasyQuestions).NotEmpty();
            RuleFor(qp => qp.NumberOfMidQuestions).NotEmpty();
            RuleFor(qp => qp.NumberOfHardQuestions).NotEmpty();
        }
    }
}

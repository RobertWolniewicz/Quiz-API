using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator(AppDB DbContext)
        {
            RuleFor(c => c.Name).NotEmpty()
                .Custom((value, context) =>
                {
                    var exist = DbContext.categories.Any(c => c.Name == value);
                    if (exist)
                    {
                        context.AddFailure("Name", "That category exist");
                    }
                });
        }
    }
}

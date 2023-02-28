using FluentValidation;
using Sieve.Models;

namespace Quiz_API.Validators
{
    public class SieveValidator : AbstractValidator<SieveModel>
    {
        public SieveValidator()
        {
            RuleFor(sm => sm.PageSize).NotEmpty();
            RuleFor(sm => sm.Page).NotEmpty();

        }
    }
}

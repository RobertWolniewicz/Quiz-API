using FluentValidation;
using Sieve.Models;

namespace Quiz_API.Validators
{
    public class SieveValidator : AbstractValidator<SieveModel>
    {
        public SieveValidator()
        {
            RuleFor(sm => sm.PageSize).NotNull().GreaterThan(0);
            RuleFor(sm => sm.Page).NotNull().GreaterThan(0);

        }
    }
}

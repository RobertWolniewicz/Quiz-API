using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;

namespace Quiz_API.Validators
{
    public class AccountValidator : AbstractValidator<RegisterUserDto>
    {
        public AccountValidator(AppDB DbContext)
        {  RuleFor(u => u.Password)
                .MinimumLength(6)
                .Equal(u => u.ConfirmedPassword);

            RuleFor(u => u.EmailAddres)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var takedEmail = DbContext.users.Any(u => u.EmailAddres == value);
                    if(takedEmail)
                    {
                        context.AddFailure("EmailAddres", "That email addres is taken");
                    }
                });
          

        }
    }
}

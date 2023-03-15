using FluentValidation;
using Quiz_API.Entity;

namespace Quiz_API.Validators
{
    public class EmailParametersValidator : AbstractValidator<EmailParams>
    {
        public EmailParametersValidator()
        {
            RuleFor(ep => ep.Email).NotEmpty();
            RuleFor(ep => ep.SmtpSerwer).NotEmpty();
            RuleFor(ep => ep.SmtpPort).NotEmpty();
            RuleFor(ep => ep.Password).NotEmpty();
            RuleFor(ep => ep.Subject).NotEmpty();
            RuleFor(ep => ep.Body).NotEmpty();
        }

    }
}

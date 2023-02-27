using FluentValidation;
using Quiz_API.Entity;

namespace Quiz_API.Validators
{
    public class EmailParametersValidator : AbstractValidator<EmailParams>
    {
        public EmailParametersValidator()
        {
            RuleFor(ep => ep.email).NotEmpty();
            RuleFor(ep => ep.smtpSerwer).NotEmpty();
            RuleFor(ep => ep.smtpPort).NotEmpty();
            RuleFor(ep => ep.password).NotEmpty();
            RuleFor(ep => ep.subject).NotEmpty();
            RuleFor(ep => ep.body).NotEmpty();
    }
            
    }
}

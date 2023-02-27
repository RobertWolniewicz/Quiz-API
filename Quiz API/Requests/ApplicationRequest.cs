using Quiz_API.Entity;
using Quiz_API.Services;
using Quiz_API.Validators;

namespace Quiz_API.Requests
{
    public static class ApplicationRequest
    {
        public static WebApplication ReqisterApplicationEndpoints(this WebApplication app)
        {
            app.MapPost("App", ApplicationRequest.Set)
                .WithTags("Application")
                .WithValidator<EmailParams>()
                .Accepts<EmailParams>("application/json");
            app.MapGet("App", ApplicationRequest.Get)
                .WithTags("Application")
                .Produces<EmailParams>();

            return app;
        }
        public static async Task<IResult> Set(IApplicationServices services, EmailParams parameters)
        {
            services.SetEmailParams(parameters);
            return Results.Ok();
        }
        public static async Task<IResult> Get(IApplicationServices services)
        {
            ;
            return Results.Ok(services.GetEmailParams());
        }
    }
}

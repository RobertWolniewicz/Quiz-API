using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;

namespace Quiz_API.Requests
{
    public static class AccountRequest
    {
        private static WebApplication _app;
        public static WebApplication ReqisterAccountEndpoints(this WebApplication app)
        {
            _app = app;
            app.MapPost("Register", AccountRequest.Register)
                .Produces(StatusCodes.Status200OK)
                .WithTags("Account")
                .WithValidator<RegisterUserDto>()
                .Accepts<RegisterUserDto>("application/json");
            app.MapPost("Login", AccountRequest.Login)
                .Produces<string>(StatusCodes.Status200OK)
                .WithTags("Account")
                .Accepts<LoginDto>("application/json");
            return app;
        }
        public static IResult Register(IAccountServices service, RegisterUserDto newUserDto, string T)
        {
            service.GetType().GetMethod("RegisterUser").MakeGenericMethod(Type.GetType("Quiz_API.Entity." + T))
                                .Invoke(service, new object[] { newUserDto });
            return Results.Ok();
        }
        public static IResult Login(IAccountServices service, LoginDto dto)
        {
            var token = service.GenereteJwt(dto, _app);
            return Results.Ok(token);
        }
    }
}

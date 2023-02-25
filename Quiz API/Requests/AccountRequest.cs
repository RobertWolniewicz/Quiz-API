using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;
using System.Reflection;

namespace Quiz_API.Requests
{
    public static class AccountRequest
    {
        private static WebApplication _app;
        private static IUserContextServices _userContextServices;
        public static WebApplication ReqisterAccountEndpoints(this WebApplication app, IUserContextServices userContextServices)
        {
            _app = app;
            _userContextServices = userContextServices;
            app.MapPost("Register", AccountRequest.Register)
                .Produces(StatusCodes.Status200OK)
                .WithTags("Account")
                .WithValidator<RegisterUserDto>()
                .Accepts<RegisterUserDto>("application/json");
            app.MapPost("Login", AccountRequest.Login)
                .Produces<string>(StatusCodes.Status200OK)
                .WithTags("Account")
                .Accepts<LoginDto>("application/json");
            app.MapDelete("Login", AccountRequest.Delete)
               .Produces(StatusCodes.Status204NoContent)
               .Produces(StatusCodes.Status404NotFound)
               .Produces(StatusCodes.Status403Forbidden)
               .WithTags("Account");

            return app;
        }
        public static async Task<IResult> Register(IAccountServices service, RegisterUserDto newUserDto, string T)
        {
            MethodInfo method = service.GetType().GetMethod("RegisterUser").MakeGenericMethod(Type.GetType("Quiz_API.Entity." + T));
            await (Task)method.Invoke(service, new object[] { newUserDto });
            return Results.Ok();
        }
        public static async Task<IResult> Login(IAccountServices service, LoginDto dto)
        {
            var token = await service.GenereteJwt(dto, _app);
            return Results.Ok(token);
        }
        public static async Task<IResult> Delete(IAccountServices service, int Id)
        {
            if (!(Id == _userContextServices.GetUserid))
            {
                return Results.Forbid();
            }
            await service.Delete(Id);
            return Results.NoContent();
        }
    }
}

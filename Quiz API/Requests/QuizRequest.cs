using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;

namespace Quiz_API.Requests
{
    public static class QuizRequest
    {
        public static WebApplication ReqisterQuizEndpoints(this WebApplication app)
        {
            app.MapGet("Quiz", QuizRequest.GetQuiz)
               .Produces<QuizQuestion>()
               .WithTags("Quiz")
               .WithValidator<QuizParameters>()
               .Accepts<QuizParameters>("application/json");
            app.MapPost("Quiz", QuizRequest.Result)
               .WithTags("Quiz")
               .Accepts<List<AnswerDto>>("application/json");
            return app;
        }
        public static async Task<IResult> GetQuiz(IQuizServices service, QuizParameters parameters)
        {
            return Results.Ok(await service.GetQuiz(parameters));
        }
        public static async Task<IResult> Result(IQuizServices service, List<AnswerDto> answers)
        {
            return Results.Ok(await service.PostResult(answers));
        }
        public static async Task<IResult> UserQuiz(IQuizServices service)
        {
            return Results.Ok(await service.GetUserQuiz());
        }
    }
}

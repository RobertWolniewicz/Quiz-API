using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;
using System.Reflection;

namespace Quiz_API.Requests
{
    public static class QuestionRequest
    {
        public static WebApplication ReqisterQuestionEndpoints(this WebApplication app)
        {
            app.MapPost("Question", QuestionRequest.Create)
                .Produces<QuestionDto>(StatusCodes.Status201Created)
                .WithTags("Question")
                .WithValidator<QuestionDto>()
                .Accepts<QuestionDto>("application/json");

            app.MapGet("Question", QuestionRequest.GetAll)
                .Produces<List<QuestionDto>>()
                .WithTags("Question");

            app.MapGet("Question/{Id}", QuestionRequest.GetById)
                .Produces<QuestionDto>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Question");

            app.MapPut("Question", QuestionRequest.Update)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Accepts<QuestionDto>("application/json")
                .WithTags("Question")
                .WithValidator<QuestionDto>();

            app.MapDelete("Question/{Id}", QuestionRequest.Delete)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Question");

            return app;
        }
        public static async Task<IResult> GetAll(IQuestionServices service)
        {
            return Results.Ok(await service.GetAll());
        }
        public static async Task<IResult> GetById(IQuestionServices service, int Id)
        {
            var result = await service.GetById(Id);
            return Results.Ok(result);
        }
        public static async Task<IResult> Delete(IQuestionServices service, int Id)
        {
            await service.Delete(Id);
            return Results.NoContent();
        }
        public static async Task<IResult> Update(IQuestionServices service, QuestionDto UpdateData)
        {
           await service.Update(UpdateData);
           return Results.NoContent();
        }
        public static async Task<IResult> Create(IQuestionServices service, QuestionDto newQuestion, string T)
        {
            MethodInfo method = service.GetType().GetMethod("Create").MakeGenericMethod(Type.GetType("Quiz_API.Entity." + T));
            var NewQuestion = await (Task<QuestionDto>)method.Invoke(service, new object[] { newQuestion });

            return Results.Created($"/Question/{NewQuestion.Id}", NewQuestion);
        }
    }
}

using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;

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
        public static IResult GetAll(IQuestionServices service)
        {
            return Results.Ok(service.GetAll());
        }
        public static IResult GetById(IQuestionServices service, int Id)
        {
            var result = service.GetById(Id);
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }
        public static IResult Delete(IQuestionServices service, int Id)
        {
            var result = service.GetById(Id);
            if (result == null)
            {
                return Results.NotFound();
            }
            service.Delete(Id);
            return Results.NoContent();
        }
        public static IResult Update(IQuestionServices service, QuestionDto UpdateData)
        {
            var result = service.GetById(UpdateData.Id);
            if (result == null)
            {
                return Results.NotFound();
            }
            service.Update(UpdateData);
            return Results.NoContent();
        }
        public static IResult Create(IQuestionServices service, QuestionDto newQuestion, string T)
        {
            var NewQuestion = (QuestionDto)service.GetType().GetMethod("Create").MakeGenericMethod(Type.GetType("Quiz_API.Entity." + T))
                                .Invoke(service, new object[] { newQuestion });
            if (NewQuestion == null)
            {
                return Results.BadRequest("This category already exists");
            }
            return Results.Created($"/Question/{NewQuestion.Id}", NewQuestion);
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;
using Sieve.Models;
using Sieve.Services;
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
                .WithValidator<NewQuestionModel>()
                .Accepts<NewQuestionModel>("application/json");

            app.MapGet("Question", QuestionRequest.GetAll)
                .Produces<PageResult<QuestionDto>>()
                .WithValidator<SieveModel>()
                .WithTags("Question");

            app.MapGet("Question/{Id}", QuestionRequest.GetById)
                .Produces<QuestionDto>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Question");

            app.MapPut("Question", QuestionRequest.Update)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Accepts<QuestionDto>("application/json")
                .WithTags("Question");

            app.MapDelete("Question/{Id}", QuestionRequest.Delete)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Question");

            return app;
        }
        public static async Task<IResult> GetAll(IQuestionServices service, [FromBody] SieveModel query, ISieveProcessor sieveprocessor)
        {
            return Results.Ok(await service.GetAll(query, sieveprocessor));
        }
        public static async Task<IResult> GetById(IQuestionServices service, int Id)
        {
            var result = await service.GetById(Id);
            return Results.Ok(result);
        }
        [Authorize(Roles = "Admin")]
        public static async Task<IResult> Delete(IQuestionServices service, int Id)
        {
            await service.Delete(Id);
            return Results.NoContent();
        }
        [Authorize(Roles = "Admin")]
        public static async Task<IResult> Update(IQuestionServices service, QuestionDto UpdateData)
        {
           await service.Update(UpdateData);
           return Results.NoContent();
        }
        [Authorize(Roles ="Admin")]
        public static async Task<IResult> Create(IQuestionServices service, NewQuestionModel newQuestion, string T)
        {
            MethodInfo method = service.GetType().GetMethod("Create").MakeGenericMethod(Type.GetType("Quiz_API.Entity." + T));
            var NewQuestion = await (Task<QuestionDto>)method.Invoke(service, new object[] { newQuestion });

            return Results.Created($"/Question/{NewQuestion.Id}", NewQuestion);
        }
    }
}

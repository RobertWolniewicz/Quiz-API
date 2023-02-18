using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDB>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizConnectionString"))
        );
builder.Services.AddScoped<IQuizServices, QuizServices>();
builder.Services.AddScoped<IQuestionServices, QuestionServices>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.MapGet("/Quiz", (IQuizServices service, [FromBody] QuizParameters param, [FromHeader]User user)
   // => service.GetQuiz( param, user));
//app.MapPost("/Quiz",(IQuizServices service, User user, List<AnswersModel> Answers) 
    //=>service.PostResult( user, Answers));
app.MapPost("/AddQuestion/{T}", (DtoQuestion Question, IQuestionServices service, string T) => 
{
    var createMethod = service.GetType().GetMethod("Create").MakeGenericMethod(Type.GetType(T));
    createMethod.Invoke(service, new object[] { Question });
});

app.Run();
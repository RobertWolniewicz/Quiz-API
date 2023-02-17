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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Questions/{category}", (IQuizServices service, QuizParameters param, Category category, User user)
    => service.GetQuiz( category, param, user));


app.MapPost("/AddQuestions/{category}", (List<Question> Questions,Category category ) =>
{

});



app.Run();


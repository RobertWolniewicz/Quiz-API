using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quiz_API;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Requests;
using Quiz_API.Services;
using Quiz_API.Validators;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CategoryValidator));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<AppDB>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizConnectionString"))
        );
builder.Services.AddScoped<IQuizServices, QuizServices>();
builder.Services.AddScoped<IQuestionServices, QuestionServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ReqisterCategoryEndpoints();
app.ReqisterQuestionEndpoints();

//app.MapGet("/Quiz", (IQuizServices service, [FromBody] QuizParameters param, [FromHeader]User user)
// => service.GetQuiz( param, user));
//app.MapPost("/Quiz",(IQuizServices service, User user, List<AnswersModel> Answers) 
//=>service.PostResult( user, Answers));
app.Run();
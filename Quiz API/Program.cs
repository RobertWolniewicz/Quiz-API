using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quiz_API;
using Quiz_API.Entity;
using Quiz_API.Middleware;
using Quiz_API.Models;
using Quiz_API.Requests;
using Quiz_API.Services;
using Quiz_API.Sieve;
using Quiz_API.Validators;
using Sieve.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CategoryValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(QuestionValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(AccountValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(QuizValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(EmailParametersValidator));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<AppDB>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuizConnectionString"))
        );
builder.Services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IApplicationServices, ApplicationServices>();
builder.Services.AddScoped<IQuizServices, QuizServices>();
builder.Services.AddScoped<IQuestionServices, QuestionServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IUserContextServices, UserContextServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
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
app.ReqisterAccountEndpoints();
app.ReqisterQuizEndpoints();
app.ReqisterApplicationEndpoints();

app.Run();
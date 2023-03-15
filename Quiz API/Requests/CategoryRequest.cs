using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;
using Sieve.Models;
using Sieve.Services;

namespace Quiz_API;

public static class CategoryRequest
{
    public static WebApplication ReqisterCategoryEndpoints(this WebApplication app)
    {
        app.MapPost("Category", CategoryRequest.Create)
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .WithTags("Category")
            .WithValidator<CategoryDto>()
            .Accepts<CategoryDto>("application/json");

        app.MapGet("Category", CategoryRequest.GetAll)
            .Produces<PageResult<CategoryDto>>()
            .WithValidator<SieveModel>()
            .WithTags("Category");

        app.MapGet("Category/{Id}", CategoryRequest.GetById)
            .Produces<CategoryDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Category");

        app.MapPut("Category", CategoryRequest.Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<CategoryDto>("application/json")
            .WithTags("Category")
            .WithValidator<CategoryDto>();

        app.MapDelete("Category/{Id}", CategoryRequest.Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Category");

        return app;
    }
    public static async Task<IResult> GetAll(ICategoryServices service, [FromBody] SieveModel query, ISieveProcessor sieveprocessor)
    {
        return Results.Ok(await service.GetAll(query, sieveprocessor));
    }
    [Authorize(Roles = "Admin")]
    public static async Task<IResult> GetById(ICategoryServices service, int Id)
    {
        var result = await service.GetById(Id);
        return Results.Ok(result);
    }
    [Authorize(Roles = "Admin")]
    public static async Task<IResult> Create(ICategoryServices service, CategoryDto newCategory)
    {
        var NewCategory = await service.Create(newCategory);
        return Results.Created($"/Category/{NewCategory.Id}", NewCategory);
    }
    [Authorize(Roles = "Admin")]
    public static async Task<IResult> Update(ICategoryServices service, CategoryDto UpdateData)
    {
        await service.Update(UpdateData);
        return Results.NoContent();
    }
    [Authorize(Roles = "Admin")]
    public static async Task<IResult> Delete(ICategoryServices service, int Id)
    {
        await service.Delete(Id);
        return Results.NoContent();
    }
}
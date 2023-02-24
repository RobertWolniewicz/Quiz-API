using FluentValidation;
using Quiz_API.Entity;
using Quiz_API.Models;
using Quiz_API.Services;
using Quiz_API.Validators;

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
            .Produces<List<CategoryDto>>()
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
    public static IResult GetAll(ICategoryServices service)
    {
        return Results.Ok(service.GetAll());
    }
    public static IResult GetById(ICategoryServices service, int Id)
    {
        var result = service.GetById(Id);
        if(result == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }
    public static IResult Create(ICategoryServices service, CategoryDto newCategory)
    {
        var NewCategory = service.Create(newCategory);
        return Results.Created($"/Category/{NewCategory.Id}",NewCategory);
    }
    public static IResult Update(ICategoryServices service, CategoryDto UpdateData)
    {
        var result = service.GetById(UpdateData.Id);
        if (result == null)
        {
            return Results.NotFound();
        }
        service.Update(UpdateData);
        return Results.NoContent();
    }
    public static IResult Delete(ICategoryServices service, int Id)
    {
        var result = service.GetById(Id);
        if (result == null)
        {
            return Results.NotFound();
        }
        service.Delete(Id);
        return Results.NoContent();
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IQuotesService, QuotesServiceImplementation>();
builder.Services.AddSingleton<IBaseBookFactory, BaseBookFactory>();
builder.Services.AddSingleton<IValidator<BaseBookDTO>, BaseBookDTO.BaseBookDTOValidator>();
builder.Services.AddSingleton<IValidator<BookWithBudgetDTO>, BookWithBudgetDTO.BookWithBudgetDTOValidator>();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Cotizaciones la ping‹inera", Version = "v1" });
    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapControllers();

app.Run();

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Services.Implementations;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<Database>(options => options.UseSqlServer(builder.Configuration["SQLConnectionString"]));
builder.Services.AddScoped<IDatabase, Database>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IBooksBudgetService, BooksBudgetService>();
builder.Services.AddScoped<ITotalPriceQuotationService, TotalPriceQuotationService>();
builder.Services.AddScoped<ITotalPriceQuotesService, TotalPriceQuotesService>();
builder.Services.AddScoped<IDataBaseService, DataBaseService>();

builder.Services.AddTransient<IBaseBookFactory, BaseBookFactory>();
builder.Services.AddSingleton<IValidator<BaseBookInputDTO>, BaseBookInputDTO.BaseBookDTOValidator>();
builder.Services.AddSingleton<IValidator<BookWithBudgeInputDTO>, BookWithBudgeInputDTO.BookWithBudgetDTOValidator>();
builder.Services.AddSingleton<IValidator<InformationInputDto>, InformationInputDto.InformationInputDtoValidator>();

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

using FluentValidation;
using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IQuotesService, QuotesServiceImplementation>();
builder.Services.AddSingleton<IBaseBookFactory, BaseBookFactory>();
builder.Services.AddSingleton<IValidator<BaseBookDTO>, BaseBookDTO.BaseBookDTOValidator>();
builder.Services.AddSingleton<IValidator<BookWithBudgetDTO>, BookWithBudgetDTO.BookWithBudgetDTOValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

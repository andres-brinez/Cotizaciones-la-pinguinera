using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Services.Implementations;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Se configura el servicio de autenticaciÛn y autorizaciÛn
builder.Services.AddAuthorization();
builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(option =>
  {
      var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSecretKey"]));

      option.RequireHttpsMetadata = false;

      option.TokenValidationParameters = new TokenValidationParameters()
      {
          ValidateIssuer = true,
          ValidateLifetime = true,
          IssuerSigningKey = signinKey,
          ValidAudience = builder.Configuration["TokenIssuer"],
          ValidIssuer = builder.Configuration["TokenIssuer"]
      };
  });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
      builder =>
      {
          builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
      });
});


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<Database>(options => options.UseSqlServer(builder.Configuration["SQLConnectionString"]));
builder.Services.AddScoped<IDatabase, Database>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IBooksBudgetService, BooksBudgetService>();
builder.Services.AddScoped<ITotalPriceQuotationService, TotalPriceQuotationService>();
builder.Services.AddScoped<ITotalPriceQuotesService, TotalPriceQuotesService>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IDataBaseService, DataBaseService>();

builder.Services.AddTransient<IBaseBookFactory, BaseBookFactory>();
builder.Services.AddSingleton<IValidator<BaseBookInputDTO>, BaseBookInputDTO.BaseBookDTOValidator>();
builder.Services.AddSingleton<IValidator<BookWithBudgeInputDTO>, BookWithBudgeInputDTO.BookWithBudgetDTOValidator>();
builder.Services.AddSingleton<IValidator<InformationInputDto>, InformationInputDto.InformationInputDtoValidator>();
builder.Services.AddSingleton<IValidator<RegisterUserInputDTO>, RegisterUserInputDTO.RegisterUserInputDTOValidator>();
builder.Services.AddSingleton<IValidator<LoginUserInputDTO>, LoginUserInputDTO.LoginUserInputDTOValidator>();

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
app.UseAuthentication();
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

using FluentValidation;
using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO
{
    public class BaseBookInputDTO
    {


        public string NameProvider { get; set; } = string.Empty;
        public int Seniority { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OriginalPrice { get; set; }
        public int Cuantity { get; set; }
        public BaseBookType Type { get; set; }


        public BaseBookInputDTO()
        {

        }

        public BaseBookInputDTO(string nameProvider, int seniority, string title, int originalPrice, int cuantity, BaseBookType type)
        {
            NameProvider = nameProvider;
            Seniority = seniority;
            Title = title;
            OriginalPrice = originalPrice;
            Cuantity = cuantity;
            Type = type;
        }

        public class BaseBookDTOValidator : AbstractValidator<BaseBookInputDTO>
        {
            public BaseBookDTOValidator()
            {
                RuleFor(x => x.NameProvider).NotNull().NotEmpty().WithMessage("El nombre del proveedor es requerido");
                RuleFor(x => x.Seniority).NotNull().WithMessage("La antiguedad del proveedor es requerida");
                RuleFor(x => x.Seniority).GreaterThan(-1).WithMessage("La antiguedad del proveedor no puede ser negativa ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("El titulo del libro es requerido");
                RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("El precio original del libro es requerido");
                RuleFor(x => x.OriginalPrice).GreaterThan(0).WithMessage("El precio original del libro debe ser mayor a 0");
                RuleFor(x => x.Cuantity).NotEmpty().WithMessage("La cantidad de libros es requerida");
                RuleFor(x => x.Cuantity).GreaterThan(0).WithMessage("La cantidad de libros debe ser mayor a 0");
                RuleFor(x => x.Type).NotNull().WithMessage("El tipo de libro es requerido");
                RuleFor(x => x.Type).IsInEnum().WithMessage("El tipo de libro no es valido");
            }
        }

    }


}

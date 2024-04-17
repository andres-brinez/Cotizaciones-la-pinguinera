using FluentValidation;
using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO
{
    public class BaseBookInputDTO
    {

        public string Id { get; set; } = string.Empty;
        public string NameProvider { get; set; } = string.Empty;
        public int Seniority { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public BaseBookType Type { get; set; }


        public BaseBookInputDTO()
        {

        }

        public BaseBookInputDTO(string id,string nameProvider, int seniority, string title, int originalPrice, int cuantity, BaseBookType type)
        {
            Id = id;
            NameProvider = nameProvider;
            Seniority = seniority;
            Title = title;
            OriginalPrice = originalPrice;
            Quantity = cuantity;
            Type = type;
        }

        public class BaseBookDTOValidator : AbstractValidator<BaseBookInputDTO>
        {
            public BaseBookDTOValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("El id del libro es requerido");

                RuleFor(x => x.NameProvider).NotNull().NotEmpty().WithMessage("El nombre del proveedor es requerido");
                RuleFor(x => x.Seniority).NotNull().WithMessage("La antiguedad del proveedor es requerida");
                RuleFor(x => x.Seniority).GreaterThan(-1).WithMessage("La antiguedad del proveedor no puede ser negativa ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("El titulo del libro es requerido");
                RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("El precio original del libro es requerido");
                RuleFor(x => x.OriginalPrice).GreaterThan(0).WithMessage("El precio original del libro debe ser mayor a 0");
                RuleFor(x => x.Quantity).NotEmpty().WithMessage("La cantidad de libros es requerida");
                RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("La cantidad de libros debe ser mayor a 0");
                RuleFor(x => x.Type).NotNull().WithMessage("El tipo de libro es requerido");
                RuleFor(x => x.Type).IsInEnum().WithMessage("El tipo de libro no es valido");
            }
        }

    }


}

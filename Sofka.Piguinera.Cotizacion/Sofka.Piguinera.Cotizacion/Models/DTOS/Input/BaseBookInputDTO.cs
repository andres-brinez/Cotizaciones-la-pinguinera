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
        public BaseBookType Type { get; set; }

        public BaseBookInputDTO()
        {

        }

        protected BaseBookInputDTO(string title, int originalPrice)
        {
            Title = title;
            OriginalPrice = originalPrice;
        }

        public class BaseBookDTOValidator : AbstractValidator<BaseBookInputDTO>
        {
            public BaseBookDTOValidator()
            {
                // name provider es requerido
                RuleFor(x => x.NameProvider).NotNull().NotEmpty().WithMessage("El nombre del proveedor es requerido");
                RuleFor(x => x.Seniority).NotNull().WithMessage("La antiguedad del proveedor es requerida");
                RuleFor(x => x.Seniority).GreaterThan(-1).WithMessage("La antiguedad del proveedor no puede ser negativa ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("El titulo del libro es requerido");
                RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("El precio original del libro es requerido");
                RuleFor(x => x.OriginalPrice).GreaterThan(0).WithMessage("El precio original del libro debe ser mayor a 0");
            }
        }

    }


}

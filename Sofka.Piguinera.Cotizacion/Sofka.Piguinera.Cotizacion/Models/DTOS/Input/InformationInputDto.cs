using FluentValidation;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.Input
{
    public class InformationInputDto
    {

        public string Id { get; set; }
        public int Cuantity { get; set; }


        public InformationInputDto() { }

        public InformationInputDto(string id, int cuantity)
        {
            Id = id;
            Cuantity = cuantity;
        }


        /// validaciones con AbstractValidator
        /// 

        public class InformationInputDtoValidator : AbstractValidator<InformationInputDto>
        {
            public InformationInputDtoValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("El id del libro es requerido");
                RuleFor(x => x.Cuantity).NotEmpty().WithMessage("La cantidad de libros es requerida");
                RuleFor(x => x.Cuantity).GreaterThan(0).WithMessage("La cantidad de libros debe ser mayor a 0");


            }
        }


    }
}

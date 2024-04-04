using FluentValidation;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO
{
    public class BookWithBudgeInputDTO
    {
        public List<BaseBookInputDTO> Books { get; set; }
        public decimal Budget { get; set; }

        public BookWithBudgeInputDTO()
        {
        }

        public BookWithBudgeInputDTO(List<BaseBookInputDTO> book, decimal budget)
        {
            Books = book;
            Budget = budget;
        }

        public class BookWithBudgetDTOValidator : AbstractValidator<BookWithBudgeInputDTO>
        {
            public BookWithBudgetDTOValidator()
            {
                RuleFor(x => x.Books).NotNull().WithMessage("La lista de libros no puede ser nula");
                RuleForEach(x => x.Books).SetValidator(new BaseBookInputDTO.BaseBookDTOValidator());

                RuleFor(x => x.Budget).GreaterThanOrEqualTo(0).WithMessage("El presupuesto no puede ser negativo");
                RuleFor(x => x.Budget).NotNull().WithMessage("El presupuesto no puede ser nulo");
                RuleFor(x => x.Budget).NotEmpty().WithMessage("El presupuesto no puede ser vacio");

            }
        }
    }

}

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
                RuleFor(x => x.Budget).GreaterThanOrEqualTo(0).WithMessage("El presupuesto no puede ser negativo");
            }
        }
    }

}

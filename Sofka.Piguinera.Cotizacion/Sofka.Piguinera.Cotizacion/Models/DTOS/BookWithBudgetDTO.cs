using FluentValidation;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS
{
    public class BookWithBudgetDTO
    {
        public List<BaseBookDTO> Books { get; set; }
        public decimal Budget { get; set; }

        public BookWithBudgetDTO()
        {
        }

        public BookWithBudgetDTO(List<BaseBookDTO> book, decimal budget)
        {
            Books = book;
            Budget = budget;
        }

        public class BookWithBudgetDTOValidator : AbstractValidator<BookWithBudgetDTO>
        {
            public BookWithBudgetDTOValidator()
            {
                RuleFor(x => x.Books).NotNull().WithMessage("La lista de libros no puede ser nula");
                RuleFor(x => x.Budget).GreaterThanOrEqualTo(0).WithMessage("El presupuesto no puede ser negativo");
            }
        }
    }

}

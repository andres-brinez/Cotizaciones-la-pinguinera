using FluentValidation;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO
{
    public class BookWithBudgeInputDTO
    {
        public List<string> IdBooks { get; set; }
        public decimal Budget { get; set; }

        public BookWithBudgeInputDTO()
        {
        }

        public BookWithBudgeInputDTO(List<string> idsBooks, decimal budget)
        {
            IdBooks = idsBooks;
            Budget = budget;
        }

        public class BookWithBudgetDTOValidator : AbstractValidator<BookWithBudgeInputDTO>
        {
            public BookWithBudgetDTOValidator()
            {

                RuleFor(x => x.IdBooks).NotNull().NotEmpty().WithMessage("El id del libro es requerido");
                RuleFor(x => x.Budget).NotEmpty().WithMessage("El presupuesto es requerido");
                RuleFor(x => x.Budget).GreaterThan(0).WithMessage("El presupuesto debe ser mayor a 0");
               

            }
        }
    }

}

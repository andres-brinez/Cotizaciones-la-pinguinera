namespace Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO
{
    public class BookWithBudgeOutputDTO
    {
        public List <BaseBookOutputDTO> Books { get; set; }
        public float Budget { get; set; }

        public BookWithBudgeOutputDTO()
        {
            
        }

        public BookWithBudgeOutputDTO(List<BaseBookOutputDTO> books, float budget)
        {
            Books = books;
            Budget = (float)System.Math.Round(budget, 2);
        }



    }
}

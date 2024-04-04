namespace Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO
{
    public class BooksPurcheseOutputDTO
    {

        public List<BaseBookOutputDTO> Books { get; set; }
        public float TotalPrice { get; set; }
        public string TypePurchase { get; set; }
        public int CountBook { get; set; }


        public BooksPurcheseOutputDTO()
        {

        }

        public BooksPurcheseOutputDTO(List<BaseBookOutputDTO> books, float totalPrice, string typePurchase, int countBook)
        {
            Books = books;
            TotalPrice = (float)System.Math.Round(totalPrice, 2);
            TypePurchase = typePurchase;
            CountBook = countBook;
        }

    }
}

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

        public BooksPurcheseOutputDTO(List<BaseBookOutputDTO> books)
        {
            Books = books;
            CountBook = Books.Sum(book => book.Cuantity);
            TypePurchase = CountBook <= 0 ? "----" : (CountBook < 10 && CountBook >= 0 ? "Compra al detal" : "Compra al por mayor");

            TotalPrice = (float)System.Math.Round(books.Sum(item => item.UnitPrice*item.Cuantity),2);



        }

    }
}

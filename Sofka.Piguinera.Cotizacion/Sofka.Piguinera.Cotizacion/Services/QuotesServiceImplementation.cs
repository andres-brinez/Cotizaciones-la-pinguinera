using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Factories;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public class QuotesServiceImplementation : IQuotesService
    {

        private readonly IBaseBookFactory _baseBookFactory;

        public QuotesServiceImplementation(IBaseBookFactory baseBookFactory)
        {
            _baseBookFactory = baseBookFactory;
        }


        // Calculate value to pay
        public string CalculateTotalPrice(BaseBookDTO payload)
        {
            //return book.CalculateTotalPrice();
            var book= _baseBookFactory.Create(payload);




            book.CalculateTotalPrice().ToString("Compra al detal");

            return book.ToString();
        }


        // Calculate value to pay
        public String CalculateTotalPrice(List<BaseBookDTO> payload)
        {

            var books = payload.Select(item=>_baseBookFactory.Create(item)).ToList();

            // el valor a pagar  por todos los ejemplares            
            var totalPrice = books.Sum(item=>item.CalculateTotalPrice());

            String typePurchase = "Compra al detal";



            foreach (var book in books)
            {

                book.CalculateTotalPrice();

                if (books.Count <= 10)
                {
                    book.CurrentPrice *= 1.02; // Incremento del 2% para compras al detal

                }
                else
                {
                    


                }

            }


            Quotes quotes = new Quotes(books, totalPrice, typePurchase);


            return quotes.ToString();

        }















    }
}

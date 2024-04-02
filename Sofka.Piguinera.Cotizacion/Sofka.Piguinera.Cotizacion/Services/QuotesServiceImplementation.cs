using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using static System.Reflection.Metadata.BlobBuilder;


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
        public string TotalPricePurchese(BaseBookDTO payload)
        {
            var book= _baseBookFactory.Create(payload);

            book.CalculateTotalPrice();

            BookPricingService.ApplyRetailIncrease(book);

            return book.ToString();
        }


        // Calculate value to pay
        public String TotalPricePurcheses(List<BaseBookDTO> payload)
        {

            var books = payload.Select(item => _baseBookFactory.Create(item)).ToList();            
            books = books.OrderByDescending(item => item.CurrentPrice).ToList();

            String typePurchase = books.Count > 10 ? "Compra al por mayor" : "Compra al detal";

            BookPricingService.CalculatePurcheseValue(books);

            var totalPrice = (float)books.Sum(item => item.CurrentPrice);

            Quotes quotes = new Quotes(books, totalPrice, typePurchase);

            return quotes.ToString();
        }

      

    }
}

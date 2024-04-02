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
        public string CalculateTotalPricePurchese(BaseBookDTO payload)
        {
            //return book.CalculateTotalPrice();
            var book= _baseBookFactory.Create(payload);

            book.CalculateTotalPrice().ToString("Compra al detal");

            return book.ToString();
        }


        // Calculate value to pay
        public String CalculateTotalPricePurchese(List<BaseBookDTO> payload)
        {
            const double RETAIL_INCREASE = 1.02;
            const double BULK_DECREASE_PER_UNIT = 0.15;

            var books = payload.Select(item => _baseBookFactory.Create(item)).ToList();            
            books = books.OrderByDescending(item => item.CurrentPrice).ToList();

            String typePurchase = books.Count > 10 ? "Compra al por mayor" : "Compra al detal";

            for (int i = 0; i < books.Count; i++)
            {
                books[i].CalculateTotalPrice();

                if (i >= 10)
                {
                    ApplyBulkDecrease(books[i], BULK_DECREASE_PER_UNIT);
                }
                else
                {
                    ApplyRetailIncrease(books[i], RETAIL_INCREASE);
                }
            }

            var totalPrice = (float)books.Sum(item => item.CurrentPrice);

            Quotes quotes = new Quotes(books, totalPrice, typePurchase);

            return quotes.ToString();
        }


        private void ApplyRetailIncrease(BaseBook book, double increase)
        {
            book.CurrentPrice *= increase;
        }

        private void ApplyBulkDecrease(BaseBook book, double decreasePerUnit)
        {
            book.Discount += (decimal)decreasePerUnit;
            var discount = book.CurrentPrice * decreasePerUnit;
            book.CurrentPrice -= discount;
        }

    }
}

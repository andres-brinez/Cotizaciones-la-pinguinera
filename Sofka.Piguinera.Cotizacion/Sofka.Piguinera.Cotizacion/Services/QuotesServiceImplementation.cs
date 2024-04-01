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

            book.CalculateTotalPrice().ToString();

            return book.ToString();
        }


        // Calculate value to pay
        public Quotes CalculateTotalPrice(List<BaseBookDTO> payload)
        {

            var books = payload.Select(item=>_baseBookFactory.Create(item)).ToList();

            // el valor a pagar  por todos los ejemplares            
            var totalPrice = books.Sum(item=>item.CalculateTotalPrice());
            // pasarlo a double


            // los descuentos aplicados
            // cantidas a pagar por cada ejemplar


            //var discount = totalPrice * Quotes.DISCOUNT_Aplicados;

            return new Quotes(books, totalPrice);









        }













    }
}

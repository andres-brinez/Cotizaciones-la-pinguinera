using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using System.Text;
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


        public string BooksBudget(BookWithBudgetDTO payload)
        {

            var books = payload.Books.Select(item => _baseBookFactory.Create(item)).ToList();

            BookPricingService.CalculatePurcheseValue(books);


            books = books.OrderByDescending(item => item.Discount).ToList();

            double totalBudgetAvailable = (double)payload.Budget;

            List<BaseBook> booksAvailable = new List<BaseBook>();

            bool hasBook = false;
            bool hasNovel = false;


            List<BaseBook> booksAvailableCopy = new List<BaseBook>();

            foreach (var book in books)
            {
                bool isAvailableBudget = totalBudgetAvailable > book.CurrentPrice;
                bool shouldAddBook = isAvailableBudget && (
                    (book.Type == BaseBookType.Novel && !hasNovel) ||
                    (book.Type == BaseBookType.Book && !hasBook) ||
                    (hasBook && hasNovel));


                if (shouldAddBook)
                {
                    booksAvailable.Add(book);
                    totalBudgetAvailable -= book.CurrentPrice;


                    if (book.Type == BaseBookType.Novel)
                    {
                        hasNovel = true;
                    }
                    else if (book.Type == BaseBookType.Book)
                    {
                        hasBook = true;
                    }
                }
                
            }

           


           
            if (booksAvailable.Count == 0)
            {
                return "No se puede comprar ningun libro con el presupuesto actual";
            }

            return showInformation(booksAvailable, totalBudgetAvailable);


        }


        public string showInformation(List<BaseBook> books, double budget)
        {
            // retorna el presupuesto  con los libros que se pueden comprar

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.Append(book.ToString());
            }

            return $"Books and novels: \n" +
                $"" +
                $"{sb.ToString()}\n" +
              //  $"- Total Prices: {TotalPrices}\n" +
                $"- Budget: {budget}";


        }

    }

}






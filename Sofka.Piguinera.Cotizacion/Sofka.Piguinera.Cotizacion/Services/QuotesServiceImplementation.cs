using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Utils;
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
        
        public string TotalPricePurchese(BaseBookDTO payload)
        {
            var book= _baseBookFactory.Create(payload);

            book.CalculateTotalPrice();

            BookPricingService.ApplyRetailIncrease(book);

            return book.ToString();
        }

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
            List<BaseBook> booksAvailable = SelectBooksWithinBudget(books, ref totalBudgetAvailable);

            string message = "No se puede comprar ningun libro con el presupuesto actual";


            return booksAvailable.Count == 0 ? message : ShowInformation.Show(booksAvailable, totalBudgetAvailable);

        }

        private List<BaseBook> SelectBooksWithinBudget(List<BaseBook> books, ref double totalBudgetAvailable)
        {
            List<BaseBook> booksAvailable = new List<BaseBook>();
            bool hasBook = false;
            bool hasNovel = false;

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


                    bool hasBookAvailable= book.Type == BaseBookType.Novel ? hasNovel = true : hasBook = true;
                }
            }

            return booksAvailable;
        }

    }

}






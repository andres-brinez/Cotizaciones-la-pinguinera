using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
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
        
        public BaseBookOutputDTO TotalPricePurchese(BaseBookInputDTO payload)
        {
            var book= _baseBookFactory.Create(payload);

            book.CalculateTotalPrice();

            BookPricingService.ApplyRetailIncrease(book);

            BaseBookOutputDTO baseBookOutputDTO = new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount);

            return baseBookOutputDTO;
        }

        public BooksPurcheseOutputDTO TotalPricePurcheses(List<BaseBookInputDTO> payload)
        {

            var books = payload.Select(item => _baseBookFactory.Create(item)).ToList();  
            
           // books = books.OrderByDescending(item => item.CurrentPrice).ToList();
            BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = books.Select(item => new BaseBookOutputDTO(item.Title, item.Type, item.CurrentPrice, item.Discount)).ToList();

            var totalPrice = (float)books.Sum(item => item.CurrentPrice);
            String typePurchase = books.Count > 10 ? "Compra al por mayor" : "Compra al detal";

            BooksPurcheseOutputDTO booksPurcheseOutputDTO = new BooksPurcheseOutputDTO(booksOutput, totalPrice, typePurchase);

            return booksPurcheseOutputDTO;
        }


        public BookWithBudgeOutputDTO BooksBudget(BookWithBudgeInputDTO payload)
        {
            var books = payload.Books.Select(item => _baseBookFactory.Create(item)).ToList();
            BookPricingService.CalculatePurcheseValue(books);
            books = books.OrderByDescending(item => item.Discount).ToList();

            List<BaseBookOutputDTO> booksOutput = books.Select(item => new BaseBookOutputDTO(item.Title, item.Type, item.CurrentPrice, item.Discount)).ToList();

            double totalBudgetAvailable = (double)payload.Budget;
            List<BaseBook> booksAvailable = SelectBooksWithinBudget(books, ref totalBudgetAvailable);


            BookWithBudgeOutputDTO bookWithBudgeOutputDTO = new BookWithBudgeOutputDTO(booksOutput, (float)totalBudgetAvailable);

            return bookWithBudgeOutputDTO;

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






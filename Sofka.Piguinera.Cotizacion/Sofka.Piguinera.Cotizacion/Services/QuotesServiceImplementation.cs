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

            BaseBookOutputDTO baseBookOutputDTO = new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice,book.Discount, book.Cuantity);

            return baseBookOutputDTO;
        }

        public BooksPurcheseOutputDTO TotalPricePurcheses(List<BaseBookInputDTO> payload)
        {

            var books = payload.Select(item => _baseBookFactory.Create(item)).ToList();  
            
            BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = books.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount, book.Cuantity)).ToList();
           
            int countBooks = books.Sum(book => book.Cuantity);

            var totalPriceBooks= (float)books.Sum(item => item.CurrentPrice);

            String typePurchase = countBooks > 10 ? "Compra al por mayor" : "Compra al detal";

            BooksPurcheseOutputDTO booksPurcheseOutputDTO = new BooksPurcheseOutputDTO(booksOutput, totalPriceBooks, typePurchase, countBooks);

            return booksPurcheseOutputDTO;
        }


        public BookWithBudgeOutputDTO BooksBudget(BookWithBudgeInputDTO payload)
        {
            var books = payload.Books.Select(item => _baseBookFactory.Create(item)).ToList();
            BookPricingService.CalculatePurcheseValue(books);

            books = books.OrderByDescending(item => item.Discount).ThenBy(item => item.CurrentPrice).ToList();
            double totalBudgetAvailable = (double)payload.Budget;

            List<BaseBookOutputDTO> booksAvailable = SelectBooksWithinBudget(books, ref totalBudgetAvailable);

            BookWithBudgeOutputDTO bookWithBudgeOutputDTO = new BookWithBudgeOutputDTO(booksAvailable, (float)totalBudgetAvailable);

            return bookWithBudgeOutputDTO;

        }

        private List<BaseBookOutputDTO> SelectBooksWithinBudget(List<BaseBook> books, ref double totalBudgetAvailable)
        {
            List<BaseBookOutputDTO> booksAvailable = new List<BaseBookOutputDTO>();
            bool hasBook = false;
            bool hasNovel = false;

            foreach (var book in books)
            {
                while (totalBudgetAvailable > book.CurrentPrice)
                {
                    bool shouldAddBook = (book.Type == BaseBookType.Novel && !hasNovel) ||
                                         (book.Type == BaseBookType.Book && !hasBook) ||
                                         (hasBook && hasNovel);

                    if (shouldAddBook)
                    {
                        UpdateBookQuantity(booksAvailable, book);
                        totalBudgetAvailable -= book.CurrentPrice;
                        ValidateTypeBook(book, ref hasBook, ref hasNovel);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return booksAvailable;
        }


        private void UpdateBookQuantity(List<BaseBookOutputDTO> booksAvailable, BaseBook book)
        {
            var existingBook = booksAvailable.FirstOrDefault(b => b.Title == book.Title && b.Type == book.Type);

            if (existingBook != null)
            {
                existingBook.Cuantity++;
            }
            else
            {
                BaseBookOutputDTO bookOutputDTO = new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount, 1);
                booksAvailable.Add(bookOutputDTO);
            }
        }

        private void ValidateTypeBook(BaseBook book, ref bool hasBook, ref bool hasNovel)
        {
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

}






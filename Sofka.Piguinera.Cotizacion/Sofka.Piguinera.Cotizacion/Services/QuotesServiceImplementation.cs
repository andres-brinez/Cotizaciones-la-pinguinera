using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
namespace Sofka.Piguinera.Cotizacion.Services
{
    public class QuotesServiceImplementation : IQuotesService
    {

        private readonly IBaseBookFactory _baseBookFactory;
        private readonly IDatabase _database;


        public QuotesServiceImplementation(IBaseBookFactory baseBookFactory, IDatabase database)
        {
            _baseBookFactory = baseBookFactory;
            _database = database;
        }
        
        public async Task<BaseBookOutputDTO> TotalPricePurchese(BaseBookInputDTO payload)
        {
            var bookEntity= _baseBookFactory.Create(payload);

            bookEntity.CalculateTotalPrice();

            BookPricingService.ApplyRetailIncrease(bookEntity);

            BaseBookOutputDTO baseBookOutputDTO = new BaseBookOutputDTO(bookEntity.Title, bookEntity.Type, bookEntity.CurrentPrice,bookEntity.Discount, bookEntity.Cuantity);

            Console.WriteLine( bookEntity.Discount);

            
            var bookPersistence = new BookPersistence
            {
                Id = bookEntity.Id,
                Title = baseBookOutputDTO.Title,
                NameProvider = bookEntity.NameProvider,
                Seniority = bookEntity.Seniority,
                OriginalPrice = bookEntity.OriginalPrice,
                Quantity = bookEntity.Cuantity,
                Type = (byte)bookEntity.Type,
                UnitPrice = bookEntity.CurrentPrice,
                Discount = bookEntity.Discount,
            };

            try
            {
                await _database.Books.AddAsync(bookPersistence);
                if (!await _database.SaveAsync())
                {
                    return null;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

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

        private List<BaseBookOutputDTO> SelectBooksWithinBudget(List<BaseBookEntity> books, ref double totalBudgetAvailable)
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


        private void UpdateBookQuantity(List<BaseBookOutputDTO> booksAvailable, BaseBookEntity book)
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

        private void ValidateTypeBook(BaseBookEntity book, ref bool hasBook, ref bool hasNovel)
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






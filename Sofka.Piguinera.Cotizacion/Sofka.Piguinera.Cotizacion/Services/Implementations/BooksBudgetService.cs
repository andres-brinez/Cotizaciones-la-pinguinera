using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using static System.Reflection.Metadata.BlobBuilder;
namespace Sofka.Piguinera.Cotizacion.Services.Implementations
{
    public class BooksBudgetService : IBooksBudgetService
    {
        private readonly IBaseBookFactory _baseBookFactory;
        private readonly IDataBaseService _databaseService;

        public BooksBudgetService(IBaseBookFactory baseBookFactory, IDataBaseService databaseService)
        {
            _baseBookFactory = baseBookFactory;
            _databaseService = databaseService;
        }


        public BookWithBudgeOutputDTO BooksBudget(BookWithBudgeInputDTO payload)
        {
            var books = new List<BaseBookEntity>();

            foreach (var idBook in payload.IdBooks)
            {
                var bookPersistence = _databaseService.GetBookById(idBook);

                if (bookPersistence != null)
                {
                    CreateAndCalculateBook createAndCalculateBook = new CreateAndCalculateBook(_baseBookFactory);
                    BaseBookEntity bookEntity = createAndCalculateBook.Entity(bookPersistence,-1);
                    books.Add(bookEntity);
                }

            }

            List<BaseBookEntity> booksResult = BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = books.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.OriginalPrice, book.Discount, book.Cuantity)).ToList();

            books = books.OrderByDescending(item => item.Discount).ThenBy(item => item.OriginalPrice).ToList();

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

            List<BaseBookEntity> booksResult = new List<BaseBookEntity>();

            foreach (var originalBook in books)
            {
                BaseBookEntity bookEntity = (BaseBookEntity)originalBook.Clone();
                bookEntity.Cuantity = 0;

                while (totalBudgetAvailable > originalBook.OriginalPrice)
                {

                    bool shouldAddBook = originalBook.Type == BaseBookType.Novel && !hasNovel ||
                                         originalBook.Type == BaseBookType.Book && !hasBook ||
                                         hasBook && hasNovel;

                    if (shouldAddBook)
                    {
                        bookEntity.Cuantity++;
                        totalBudgetAvailable -= originalBook.OriginalPrice;
                        ValidateTypeBook(originalBook, ref hasBook, ref hasNovel);
                    }
                    else
                    {
                        break;

                    }
                }

                if (bookEntity.Cuantity > 0)
                {
                    booksResult.Add(bookEntity);
                }

            }

              List<BaseBookEntity> booksResult2 = BookPricingService.CalculatePurcheseValue(booksResult);
            booksAvailable = booksResult.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.OriginalPrice, book.Discount, book.Cuantity)).ToList();
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






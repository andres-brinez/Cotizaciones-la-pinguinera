using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using System.Collections.Generic;
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
            double totalBudgetAvailable = (double)payload.Budget;


            foreach (var idBook in payload.IdBooks)
            {
                var bookPersistence = _databaseService.GetBookById(idBook);

                if (bookPersistence != null)
                {
                    CreateAndCalculateBook createAndCalculateBook = new CreateAndCalculateBook(_baseBookFactory);
                    BaseBookEntity bookEntity = createAndCalculateBook.BookPersistenceToEntity(bookPersistence,-1);
                    books.Add(bookEntity);
                }

            }


            books = books.OrderByDescending(item => item.Discount).ThenBy(item => item.OriginalPrice).ToList();
            List<BaseBookOutputDTO> booksAvailable = SelectBooksWithinBudget(books, ref totalBudgetAvailable);
            BookWithBudgeOutputDTO bookWithBudgeOutputDTO = new BookWithBudgeOutputDTO(booksAvailable, (float)totalBudgetAvailable);

            return bookWithBudgeOutputDTO;

        }

        private List<BaseBookOutputDTO> SelectBooksWithinBudget(List<BaseBookEntity> books, ref double totalBudgetAvailable)
        {

            List<BaseBookOutputDTO> booksOutputDTO = new List<BaseBookOutputDTO>();
            List<BaseBookEntity> booksAvailable = new List<BaseBookEntity>();
            List<BaseBookEntity> booksPurchese = new List<BaseBookEntity>();

            bool hasBook = false;
            bool hasNovel = false;

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
                    booksAvailable.Add(bookEntity);
                }
                
            }

            booksPurchese = BookPricingService.CalculatePurcheseValue(booksAvailable);
            booksOutputDTO = booksPurchese.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.OriginalPrice, book.Discount, book.Cuantity)).ToList();
            return booksOutputDTO;
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






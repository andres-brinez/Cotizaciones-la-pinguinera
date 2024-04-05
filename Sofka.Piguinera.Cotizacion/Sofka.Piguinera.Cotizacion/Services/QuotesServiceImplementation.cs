using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
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

        public BooksPurcheseOutputDTO TotalPricePurcheses(List<InformationInputDto> payload)
        {
            var books = new List<BaseBookEntity>();


            foreach (var informationBook in payload)
            {
                BookPersistence bookPersistence = new BookPersistence();

                bookPersistence = _database.Books.FirstOrDefault(b => b.Id == informationBook.Id); // obtiene el libro de la base de datos

            
                if (bookPersistence != null)
                {
                    var baseBookFactory = new BaseBookFactory();

                    // Se pasa la información del libro de la base de datos a un objeto de tipo BaseBookEntity

                    BaseBookEntity bookEntity = baseBookFactory.Create(
                        new BaseBookInputDTO
                        {
                            Id = bookPersistence.Id,
                            Title = bookPersistence.Title,
                            OriginalPrice = (int)bookPersistence.OriginalPrice,
                            NameProvider = bookPersistence.NameProvider,
                            Seniority = (int)bookPersistence.Seniority,
                            Quantity = informationBook.Cuantity,
                            Type = (BaseBookType)bookPersistence.Type
                        }
                    );


                    bookEntity.CurrentPrice = (float)bookPersistence.UnitPrice* informationBook.Cuantity;
                    bookEntity.Discount = (float)bookPersistence.Discount;
                    bookEntity.CalculateTotalPrice();

                    books.Add(bookEntity);
                }
             
            }
 
            BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = books.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount, book.Cuantity)).ToList();
            
            BooksPurcheseOutputDTO booksPurcheseOutputDTO = new BooksPurcheseOutputDTO(booksOutput);

            return booksPurcheseOutputDTO;
        }


        public BookWithBudgeOutputDTO BooksBudget(BookWithBudgeInputDTO payload)
        {

            var books = new List<BaseBookEntity>();

            foreach (var idBook in payload.IdBooks)
            {
                BookPersistence bookPersistence = new BookPersistence();

                bookPersistence = _database.Books.FirstOrDefault(b => b.Id == idBook); // obtiene el libro de la base de datos

                if (bookPersistence != null)
                {
                    var baseBookFactory = new BaseBookFactory();

                    // Se pasa la información del libro de la base de datos a un objeto de tipo BaseBookEntity

                    BaseBookEntity bookEntity = baseBookFactory.Create(
                        new BaseBookInputDTO
                        {
                            Id = bookPersistence.Id,
                            Title = bookPersistence.Title,
                            OriginalPrice = (int)bookPersistence.OriginalPrice,
                            NameProvider = bookPersistence.NameProvider,
                            Seniority = (int)bookPersistence.Seniority,
                            Quantity = (int)bookPersistence.Quantity,
                            Type = (BaseBookType)bookPersistence.Type
                        }
                    );


                    bookEntity.CurrentPrice = (float)((float)bookPersistence.UnitPrice * bookPersistence.Quantity);
                    bookEntity.Discount = (float)bookPersistence.Discount;
                    
                    bookEntity.CalculateTotalPrice();

                    books.Add(bookEntity);
                }

            }

            BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = books.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount, book.Cuantity)).ToList();

            BooksPurcheseOutputDTO booksPurcheseOutputDTO = new BooksPurcheseOutputDTO(booksOutput);


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

                    Console.WriteLine($"Total Budget Available: {totalBudgetAvailable}");
                    Console.WriteLine($"Current Price: {book.CurrentPrice}");
                    Console.WriteLine($"prueba:{totalBudgetAvailable} >{book.CurrentPrice}");
                    Console.WriteLine(totalBudgetAvailable > book.CurrentPrice);


                    bool shouldAddBook = (book.Type == BaseBookType.Novel && !hasNovel) ||
                                         (book.Type == BaseBookType.Book && !hasBook) ||
                                         (hasBook && hasNovel);

                    Console.WriteLine($"Should Add Book: {shouldAddBook}");

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






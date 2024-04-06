using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public class TotalPriceQuotesService:ITotalPriceQuotesService
    {

        private readonly IDatabase _database;

        public TotalPriceQuotesService(IDatabase database)
        {
            _database = database;
        }

        public BooksPurcheseOutputDTO CalculateTotalPriceQuotes(List<InformationInputDto> payload)
        {
            var books = new List<BaseBookEntity>();


            foreach (var informationBook in payload)
            {
                BookPersistence bookPersistence = new BookPersistence();


                try
                {
                    bookPersistence = _database.Books.FirstOrDefault(b => b.Id == informationBook.Id); // obtiene el libro de la base de datos
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }

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


                    bookEntity.CurrentPrice = (float)bookPersistence.UnitPrice * informationBook.Cuantity;
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



    }
}

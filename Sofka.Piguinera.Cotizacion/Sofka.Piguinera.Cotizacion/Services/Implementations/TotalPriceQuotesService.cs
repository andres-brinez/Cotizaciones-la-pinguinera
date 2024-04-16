using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services.Implementations
{
    public class TotalPriceQuotesService : ITotalPriceQuotesService
    {

        private readonly IBaseBookFactory _baseBookFactory;
        private readonly IDataBaseService _databaseService;

        public TotalPriceQuotesService(IBaseBookFactory baseBookFactory, IDataBaseService databaseService)
        {
            _baseBookFactory = baseBookFactory;
            _databaseService = databaseService;
        }

        public BooksPurcheseOutputDTO CalculateTotalPriceQuotes(List<InformationInputDto> payload)
        {
            var books = new List<BaseBookEntity>();

            foreach (var informationBook in payload)
            {
                var bookPersistence = _databaseService.GetBookById(informationBook.Id);

                if (bookPersistence != null)
                {
                   
                    CreateAndCalculateBook createAndCalculateBook = new CreateAndCalculateBook(_baseBookFactory);
                    BaseBookEntity bookEntity = createAndCalculateBook.Entity(bookPersistence,informationBook.Cuantity);
                    books.Add(bookEntity);
                }

            }

   
            List<BaseBookEntity> booksResult=BookPricingService.CalculatePurcheseValue(books);

            List<BaseBookOutputDTO> booksOutput = booksResult.Select(book => new BaseBookOutputDTO(book.Title, book.Type, book.CurrentPrice, book.Discount, book.Cuantity)).ToList();

            BooksPurcheseOutputDTO booksPurcheseOutputDTO = new BooksPurcheseOutputDTO(booksOutput);

            return booksPurcheseOutputDTO;
        }



    }
}

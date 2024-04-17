using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services.Implementations
{
    public class TotalPriceQuotationService : ITotalPriceQuotationService
    {

        private readonly IBaseBookFactory _baseBookFactory;
        private readonly IDataBaseService _databaseService;

        public TotalPriceQuotationService(IBaseBookFactory baseBookFactory, IDataBaseService databaseService)
        {
            _baseBookFactory = baseBookFactory;
            _databaseService = databaseService;
        }

        public async Task<BaseBookOutputDTO> CalculateTotalPriceQuotation(BaseBookInputDTO payload)
        {
            var bookEntity = _baseBookFactory.CreateBookEntity(payload.Type, payload.Id, payload.Title, payload.OriginalPrice, payload.NameProvider, payload.Seniority, payload.Quantity);

            // Se calcula el precio  del libro con los descuentos aplicados 
            bookEntity.CalculateTotalPrice();

            BaseBookOutputDTO baseBookOutputDTO = new BaseBookOutputDTO(bookEntity.Title, bookEntity.Type, bookEntity.CurrentPrice, bookEntity.Discount, bookEntity.Cuantity);
            var bookPersistence = _baseBookFactory.BookEntityToPersistence(bookEntity);

            if (!await _databaseService.AddBookAsync(bookPersistence))
            {
                return null;
            }

            return baseBookOutputDTO;
        }
    }
}

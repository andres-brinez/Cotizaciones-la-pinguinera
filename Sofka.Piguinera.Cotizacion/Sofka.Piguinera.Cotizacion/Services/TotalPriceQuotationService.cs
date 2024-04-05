using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public class TotalPriceQuotationService : ITotalPriceQuotationService
    {

        private readonly IBaseBookFactory _baseBookFactory;
        private readonly IDatabase _database;

        public TotalPriceQuotationService(IBaseBookFactory baseBookFactory, IDatabase database)
        {
            _baseBookFactory = baseBookFactory;
            _database = database;
        }

        public async Task<BaseBookOutputDTO> CalculateTotalPriceQuotation(BaseBookInputDTO payload)
        {

            var bookEntity = _baseBookFactory.Create(payload);

            bookEntity.CalculateTotalPrice();


            BookPricingService.ApplyRetailIncrease(bookEntity);

            BaseBookOutputDTO baseBookOutputDTO = new BaseBookOutputDTO(bookEntity.Title, bookEntity.Type, bookEntity.CurrentPrice, bookEntity.Discount, bookEntity.Cuantity);

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

    }
}

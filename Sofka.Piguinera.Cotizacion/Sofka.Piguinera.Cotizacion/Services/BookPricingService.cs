using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public  static class BookPricingService
    {
        public static List<BaseBook> CalculatePurcheseValue(List<BaseBook> books)
        {

            for (int i = 0; i < books.Count; i++)
            {
                books[i].CalculateTotalPrice();

                if (i >= 10)
                {
                    ApplyBulkDecrease(books[i]);
                }
                else
                {
                    ApplyRetailIncrease(books[i]);
                }
            }

            return books;
        }

        public static void ApplyRetailIncrease(BaseBook book)
        {
            const double RETAIL_INCREASE = 1.02;
            book.CurrentPrice *= RETAIL_INCREASE;
        }

        private static  void ApplyBulkDecrease(BaseBook book)
        {
            const double BULK_DECREASE_PER_UNIT = 0.15;
            var discount = book.CurrentPrice * BULK_DECREASE_PER_UNIT;

            book.Discount += (decimal)BULK_DECREASE_PER_UNIT;
            book.CurrentPrice -= discount;
        }
    }

}

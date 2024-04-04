using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public  static class BookPricingService
    {
        public static List<BaseBookEntity> CalculatePurcheseValue(List<BaseBookEntity> books)
        {

            int countBooks = 0;

            for (int i = 0; i < books.Count; i++)
            {
                books[i].CalculateTotalPrice();
                countBooks += books[i].Cuantity;


                if (i >= 10 || countBooks > 10)
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

        public static void ApplyRetailIncrease(BaseBookEntity book)
        {
            const float RETAIL_INCREASE = 1.02f;
            book.CurrentPrice *= RETAIL_INCREASE;
        }

        private static  void ApplyBulkDecrease(BaseBookEntity book)
        {
            const float BULK_DECREASE_PER_UNIT = 0.15f;
            var discount = book.CurrentPrice * BULK_DECREASE_PER_UNIT;

            book.Discount += (float)BULK_DECREASE_PER_UNIT;
            book.CurrentPrice -= discount;

        }
    }

}

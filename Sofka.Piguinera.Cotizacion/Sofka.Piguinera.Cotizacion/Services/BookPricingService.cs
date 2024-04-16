using Microsoft.AspNetCore.Http.Connections;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public static class BookPricingService
    {
        public static List<BaseBookEntity> CalculatePurcheseValue(List<BaseBookEntity> books)
        {
            int countBooks = 0;
            List<BaseBookEntity> booksResult = new List<BaseBookEntity>();

            foreach (var originalBook in books)
            {
                BaseBookEntity retailBook = (BaseBookEntity)originalBook.Clone();
                retailBook.Cuantity = 0;

                BaseBookEntity bulkBook = (BaseBookEntity)originalBook.Clone();
                bulkBook.Cuantity = 0;

                for (int j = 0; j < originalBook.Cuantity; j++)
                {
                    if (countBooks > 9)
                    {
                        bulkBook.Cuantity++;
                    }
                    else
                    {
                        retailBook.Cuantity++;
                    }

                    countBooks++;
                }

                ApplyRetailIncrease(retailBook);
                ApplyBulkDecrease(bulkBook);


                if (retailBook.Cuantity > 0)booksResult.Add(retailBook);
                if (bulkBook.Cuantity > 0)booksResult.Add(bulkBook); 
            }

            return booksResult;
        }

        public static void ApplyRetailIncrease(BaseBookEntity book)
        {
            const float RETAIL_INCREASE = 1.02f;
            book.CurrentPrice *= RETAIL_INCREASE;
        }

        private static void ApplyBulkDecrease(BaseBookEntity book)
        {
            const float BULK_DECREASE_PER_UNIT = 0.15f;
            var discount = book.CurrentPrice * BULK_DECREASE_PER_UNIT;

            book.Discount += (float)BULK_DECREASE_PER_UNIT;
            book.CurrentPrice -= discount;

        }


    }

}

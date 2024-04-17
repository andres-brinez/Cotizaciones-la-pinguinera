using Microsoft.AspNetCore.Http.Connections;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Utils
{
    public static class BookCalculatePricing
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


                if (retailBook.Cuantity > 0) booksResult.Add(retailBook);
                if (bulkBook.Cuantity > 0) booksResult.Add(bulkBook);
            }

            return booksResult;
        }

        public static void ApplyRetailIncrease(BaseBookEntity book)
        {
            const float RETAIL_INCREASE = 1.02f;
            book.OriginalPrice = Convert.ToInt32(RETAIL_INCREASE* book.OriginalPrice);
        }

        private static void ApplyBulkDecrease(BaseBookEntity book)
        {
            const float BULK_DECREASE_PER_UNIT = 0.15f;

            float amountDiscount = book.OriginalPrice * BULK_DECREASE_PER_UNIT;
            book.OriginalPrice -= Convert.ToInt32(amountDiscount);

            book.Discount += (float)BULK_DECREASE_PER_UNIT;


        }


    }

}

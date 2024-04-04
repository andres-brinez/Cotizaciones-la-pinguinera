using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Novel : BaseBook
    {
        public Novel(string title, int originalPrice, string nameProvider, int seniority, int cuantity) : base(title, originalPrice, nameProvider, seniority, cuantity, BaseBookType.Novel)
        {
        }

        public override void CalculateTotalPrice()
        {

            const float CURRENT_INCREASE = 2;

            CalculateDiscountSeniority();
           
            CurrentPrice = (float)(OriginalPrice * CURRENT_INCREASE * (1 - Discount));

            Console.WriteLine(OriginalPrice * CURRENT_INCREASE * (1 - Discount));


        }


    }


}

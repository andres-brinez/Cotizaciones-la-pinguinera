using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Novel : BaseBook
    {
        public Novel(string title, int originalPrice, string nameProvider, int seniority)
         : base(title, originalPrice, nameProvider, seniority, BaseBookType.Novel)
        {}
   
        public override void CalculateTotalPrice()
        {

            const float CURRENT_INCREASE = 2;

            Console.WriteLine("Calculating total price for Novel");

            Console.WriteLine("Original Price: " + OriginalPrice);

            CalculateDiscountSeniority();
           
            CurrentPrice = (float)(OriginalPrice * CURRENT_INCREASE * (1 - Discount));

            Console.WriteLine(OriginalPrice * CURRENT_INCREASE * (1 - Discount));

            Console.WriteLine("Total Price: " + CurrentPrice);

        }


    }


}

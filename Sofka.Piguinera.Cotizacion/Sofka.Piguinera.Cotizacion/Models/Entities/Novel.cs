namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Novel : BaseBook
    {
        public Novel(string title, int originalPrice, string nameProvider, int seniority) : base(title, originalPrice, nameProvider, seniority)
        {
        }

        public override float CalculateTotalPrice()
        {

            const decimal CURRENT_INCREASE = 2;

            CalculateDiscount();
           
            CurrentPrice = (double)(OriginalPrice * CURRENT_INCREASE * (1 - Discount));

            return (float)CurrentPrice;
        }


    }


}

using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class NovelEntity : BaseBookEntity
    {
        public NovelEntity(string id,string title, int originalPrice, string emailProvider, int seniority, int cuantity) : base(id,title, originalPrice, emailProvider, seniority, cuantity, BaseBookType.Novel)
        {
        }

        public override void CalculateTotalPrice()
        {

            const float CURRENT_INCREASE = 2;

            CalculateDiscountSeniority();
           
            CurrentPrice = (float)(UnitPrice * CURRENT_INCREASE * (1 - Discount));

        }


    }


}

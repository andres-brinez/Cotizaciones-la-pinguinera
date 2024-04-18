using Sofka.Piguinera.Cotizacion.Models.Enums;
using System;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class BookEntity : BaseBookEntity
    {

      

        public BookEntity(string id, string title, int originalPrice, string emailProvider, int seniority, int cuantity) : base(id,title, originalPrice, emailProvider, seniority, cuantity, BaseBookType.Book)
        {
        }

        public override void CalculateTotalPrice()
        {

            const  float CURRENT_INCREASE  = 0.33f;

            CalculateDiscountSeniority();
            
            var totalPrice = (UnitPrice + (UnitPrice* CURRENT_INCREASE));
            CurrentPrice = (totalPrice * (1 - Discount));

        
        }


      
    }
    
    
}

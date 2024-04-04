using Sofka.Piguinera.Cotizacion.Models.Enums;
using System;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Book : BaseBook
    {
        public Book(string title, int originalPrice, string nameProvider, int seniority, int cuantity) : base(title, originalPrice, nameProvider, seniority, cuantity, BaseBookType.Book)
        {
        }

        public override void CalculateTotalPrice()
        {

            const  float CURRENT_INCREASE  = 0.33f;

            CalculateDiscountSeniority();
            
            var totalPrice = (OriginalPrice + (OriginalPrice* CURRENT_INCREASE));
            CurrentPrice = (totalPrice * (1 - Discount));

        
        }


      
    }
    
    
}

using System;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Book : BaseBook
    {
        public Book(string title, int originalPrice, string nameProvider, int seniority) : base(title, originalPrice, nameProvider, seniority)
        {
        }

        public override float CalculateTotalPrice()
        {

            const  Double CURRENT_INCREASE  = 0.33;

            CalculateDiscountSeniority();


            //El precio de los libros es aumentado 1/3 de su precio original,
            decimal  totalPrice = (decimal)(OriginalPrice + (OriginalPrice* CURRENT_INCREASE));

            CurrentPrice = (double)(totalPrice * (1 - Discount));

            return (float)CurrentPrice ;
        
        }


      
    }
    
    
}

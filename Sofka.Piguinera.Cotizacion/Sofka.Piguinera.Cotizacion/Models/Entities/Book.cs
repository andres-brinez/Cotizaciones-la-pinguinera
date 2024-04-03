using Sofka.Piguinera.Cotizacion.Models.Enums;
using System;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Book : BaseBook
    {
        public Book(string title, int originalPrice, string nameProvider, int seniority)
         : base(title, originalPrice, nameProvider, seniority, BaseBookType.Book) 
        {
        }
    

        public override void CalculateTotalPrice()
        {

            const  float CURRENT_INCREASE  = 0.33f;

            Console.WriteLine("Calculating total price for Book");

            Console.WriteLine("Original Price: " + OriginalPrice);
            CalculateDiscountSeniority();

            var totalPrice = (OriginalPrice + (OriginalPrice* CURRENT_INCREASE));
           Console.WriteLine("Total Price: " + totalPrice);
            

            CurrentPrice = (totalPrice * (1 - Discount));

        
        }


      
    }
    
    
}

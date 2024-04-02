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

            CalculateDiscount();

            //El precio de los libros es aumentado 1/3 de su precio original,
            decimal  totalPrice = (decimal)(OriginalPrice + (OriginalPrice* CURRENT_INCREASE));

            Console.WriteLine("El precio total del libro es: " + totalPrice);
            Console.WriteLine("El descuento aplicado es: " + Discount*100 + "%");

            CurrentPrice = (double)(totalPrice * (1 - Discount));
            Console.WriteLine("El precio actual del libro es: " + CurrentPrice);

            return (float)CurrentPrice ;
        
        }


      
    }
    
    
}

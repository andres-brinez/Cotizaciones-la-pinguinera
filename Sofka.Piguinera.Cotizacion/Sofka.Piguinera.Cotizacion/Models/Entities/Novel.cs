﻿using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Novel : BaseBook
    {
        public Novel(string title, int originalPrice, string nameProvider, int seniority)
         : base(title, originalPrice, nameProvider, seniority, BaseBookType.Novel)
        {}
   
        public override float CalculateTotalPrice()
        {

            const decimal CURRENT_INCREASE = 2;

            CalculateDiscountSeniority();
           
            CurrentPrice = (double)(OriginalPrice * CURRENT_INCREASE * (1 - Discount));

            return (float)CurrentPrice;
        }


    }


}

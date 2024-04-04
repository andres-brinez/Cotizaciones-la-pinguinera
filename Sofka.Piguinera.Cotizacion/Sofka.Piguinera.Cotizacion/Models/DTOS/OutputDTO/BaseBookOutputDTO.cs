using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO
{
    public class BaseBookOutputDTO
    {

        public string Title { get; set; }
        public BaseBookType Type { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public int Cuantity { get; set; }
        public string Discount { get; set; }

        public BaseBookOutputDTO()
        {

        }
        public BaseBookOutputDTO(string title, BaseBookType type,  float price, float discount, int cuantity)
        {

            Title = title;
            Type = type;

            UnitPrice = (float)System.Math.Round(price, 2);
            Cuantity = cuantity;
            TotalPrice = (float)System.Math.Round(UnitPrice * Cuantity, 2);

            float discountPercentage = discount * 100;
            Discount = discountPercentage.ToString() + "%";
            Cuantity = cuantity;
        }

    }
}

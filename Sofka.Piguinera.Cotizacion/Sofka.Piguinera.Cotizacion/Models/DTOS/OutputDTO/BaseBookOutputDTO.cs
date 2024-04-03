using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO
{
    public class BaseBookOutputDTO
    {

        public string Title { get; set; }
        public BaseBookType Type { get; set; }
        public float Price { get; set; }
        public string Discount { get; set; }

        public BaseBookOutputDTO()
        {

        }
        public BaseBookOutputDTO(string title, BaseBookType type,  float price, float discount)
        {

            Title = title;
            Type = type;
            Price = (float)System.Math.Round(price, 2);

            float discountPercentage = discount * 100;
            Discount = discountPercentage.ToString() + "%";
        }

    }
}

using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Strategy;
using static System.Reflection.Metadata.BlobBuilder;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public abstract class BaseBook
    {

        public string Title { get; set; }
        public int OriginalPrice { get; set; }
        public float CurrentPrice { get; set;}
        public string NameProvider { get; set; } = string.Empty;
        public int Seniority { get; set; }
        public float Discount { get; set; } = 0;
        public int Cuantity { get; set; }
        public BaseBookType Type { get; set; } 


        private readonly List<IDiscountStrategy> _discountStrategies = new List<IDiscountStrategy>();


        public BaseBook()
        {
            
        }

        protected BaseBook(string title, int originalPrice, string nameProvider, int seniority, int cuantity,BaseBookType type)
        {
            Title = title;
            OriginalPrice = originalPrice;
            NameProvider = nameProvider;
            Seniority = seniority;
            Cuantity = cuantity;
            Type = type;

            _discountStrategies = new List<IDiscountStrategy>
            {
                new SeniorityOneTwoDiscountStrategy(),
                new SeniorityMoreThanTwoDiscountStrategy()
            };

        }

        public abstract void CalculateTotalPrice();


        public void CalculateDiscountSeniority()
        {

            foreach (var strategy in _discountStrategies)
            {

                if (strategy.CanApply(Seniority))
                {
                    Discount = strategy.Apply();
                    break;
                }
            }
        }


        public override string ToString()
        {
            return $" - Title: {Title}, Type: {Type} Price: {CurrentPrice}, Discount: {Discount*100}% \n";
        }

    }
}

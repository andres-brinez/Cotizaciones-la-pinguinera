using Sofka.Piguinera.Cotizacion.DesignPattern.Strategy;
using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public abstract class BaseBookEntity
    {

        public string Id { get; set; } = string.Empty;
        public string Title { get; set; }
        public int UnitPrice { get; set; }
        public float CurrentPrice { get; set; }
        public string NameProvider { get; set; } = string.Empty;
        public int Seniority { get; set; }
        public float Discount { get; set; } = 0;
        public int Cuantity { get; set; }
        public BaseBookType Type { get; set; }


        private readonly List<IDiscountStrategy> _discountStrategies = new List<IDiscountStrategy>();


        public BaseBookEntity(string id, string title, int originalPrice, string nameProvider, int seniority, int cuantity, BaseBookType type)
        {
            Id = id;
            Title = title;
            UnitPrice = originalPrice;
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
            return $"Id: {Id}, Title: {Title}, OriginalPrice: {UnitPrice}, CurrentPrice: {CurrentPrice}, NameProvider: {NameProvider}, Seniority: {Seniority}, Discount: {Discount}, Cuantity: {Cuantity}, Type: {Type}";
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

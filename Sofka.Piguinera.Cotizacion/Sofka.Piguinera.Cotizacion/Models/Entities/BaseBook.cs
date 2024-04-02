using Sofka.Piguinera.Cotizacion.Models.Strategy;
using static System.Reflection.Metadata.BlobBuilder;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public abstract class BaseBook
    {

        public string Title { get; set; }
        public int OriginalPrice { get; set; }
        public Double CurrentPrice { get; set;}

        public string NameProvider { get; set; } = string.Empty;
        public int Seniority { get; set; }

        public decimal Discount { get; set; } = 0;

        private readonly List<IDiscountStrategy> _discountStrategies = new List<IDiscountStrategy>();


        public BaseBook()
        {
            
        }

        protected BaseBook(string title, int originalPrice, string nameProvider, int seniority)
        {
            Title = title;
            OriginalPrice = originalPrice;
            NameProvider = nameProvider;
            Seniority = seniority;

            _discountStrategies = new List<IDiscountStrategy>
        {
            new SeniorityOneTwoDiscountStrategy(),
            new SeniorityMoreThanTwoDiscountStrategy()
        };


        }


        // Calcula el precio individual de cada libro
        public abstract float CalculateTotalPrice();


        public void CalculateDiscount()
        {
            Console.WriteLine("Calculando descuento");

            foreach (var strategy in _discountStrategies)
            {
                Console.WriteLine("Seniority: " + Seniority);
                Console.WriteLine(strategy.CanApply(Seniority));
                if (strategy.CanApply(Seniority))
                {
                    Discount = strategy.Apply();
                    Console.WriteLine(Discount);
                    break;
                }
            }
        }


        public override string ToString()
        {
            return $" - Title: {Title}, Price: {CurrentPrice}, Discount: {Discount*100}% \n";
        }







    }
}

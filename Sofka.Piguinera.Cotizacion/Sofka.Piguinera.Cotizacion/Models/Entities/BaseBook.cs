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

        public BaseBook()
        {
            
        }

        protected BaseBook(string title, int originalPrice, string nameProvider, int seniority)
        {
            Title = title;
            OriginalPrice = originalPrice;
            NameProvider = nameProvider;
            Seniority = seniority;
        }

        protected BaseBook(string title, int originalPrice)
        {
            Title = title;
            OriginalPrice = originalPrice;
        }


        // Calcula el precio individual de cada libro
        public abstract float CalculateTotalPrice();

        
        public void  CalculateDiscount()
        {

            if (Seniority >= 1 && Seniority <= 2)
            {
                Discount = 0.12m;
            }
            else if (Seniority > 2)
            {
                Discount = 0.17m;
            }
        } 


        public override string ToString()
        {
            return $"Title: {Title},  Price: {CurrentPrice}";
        }


      


    }
}

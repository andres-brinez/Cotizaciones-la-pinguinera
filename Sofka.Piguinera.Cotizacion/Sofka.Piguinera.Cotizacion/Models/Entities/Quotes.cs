using System.Text;

namespace Sofka.Piguinera.Cotizacion.Models.Entities
{
    public class Quotes
    {


        // Tiene la información de los libros que se van a cotizar

        public List<BaseBook> Books { get; set; }

        //typePurchase
        public  string TypePurchase { get; set; }


        public float TotalPrices { get; set; }

        //public const float DISCOUNT_Aplicados = 0.19f;

        public Quotes()
        {
        }

        public Quotes(List<BaseBook> books,float totalPrices,string typePurchase)
        {
            Books = books;
            TotalPrices = totalPrices;
            TypePurchase = typePurchase;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var book in Books)
            {
                sb.Append(book.ToString());
            }

            return $"Books and novels: \n" +
                $"" +
                $"{sb.ToString()}" +
                $"- TotalPrices: {TotalPrices}\n"+
                $"- TypePurchase: {TypePurchase}";



        }



    }
}

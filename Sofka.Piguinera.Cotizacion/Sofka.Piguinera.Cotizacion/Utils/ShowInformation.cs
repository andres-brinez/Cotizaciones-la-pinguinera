using Sofka.Piguinera.Cotizacion.Models.Entities;
using System.Text;

namespace Sofka.Piguinera.Cotizacion.Utils
{
    public class ShowInformation
    {

        public static string Show(List<BaseBook> books, double budget)
        {
            // retorna el presupuesto  con los libros que se pueden comprar

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.Append(book.ToString());
            }

            return $"Books and novels: \n" +
                $"" +
                $"{sb.ToString()}\n" +
                //  $"- Total Prices: {TotalPrices}\n" +
                $"- Budget: {budget}";


        }
    }
}

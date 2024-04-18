namespace Sofka.Piguinera.Cotizacion.Models.Persistence
{
    public class BookPersistence
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? EmailProvider { get; set; }
        public int? OriginalPrice { get; set; }
        public int? Quantity { get; set; }
        public byte Type { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; } // Cambiado de float a double

        public string ToString()
        {
            return "Id: " + Id + " Title: " + Title + " EmailProvider: " + EmailProvider + " OriginalPrice: " + OriginalPrice + " Quantity: " + Quantity + " Type: " + Type + " UnitPrice: " + UnitPrice + " Discount: " + Discount;
        }
    }
}

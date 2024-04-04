namespace Sofka.Piguinera.Cotizacion.Models.Persistence
{
    public class BookPersistence
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? NameProvider { get; set; }
        public int? Seniority { get; set; }
        public int? OriginalPrice { get; set; }
        public int? Quantity { get; set; }
        public byte Type { get; set; }
        public double UnitPrice { get; set; }
        public float Discount { get; set; }

    }
}

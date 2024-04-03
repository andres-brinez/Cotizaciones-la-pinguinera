namespace Sofka.Piguinera.Cotizacion.Models.Strategy
{
    public interface IDiscountStrategy
    {

        bool CanApply(int seniority);
        float Apply();

    }
}

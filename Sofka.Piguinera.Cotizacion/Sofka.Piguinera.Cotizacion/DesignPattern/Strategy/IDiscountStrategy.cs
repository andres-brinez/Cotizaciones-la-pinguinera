namespace Sofka.Piguinera.Cotizacion.DesignPattern.Strategy
{
    public interface IDiscountStrategy
    {

        bool CanApply(int seniority);
        float Apply();

    }
}

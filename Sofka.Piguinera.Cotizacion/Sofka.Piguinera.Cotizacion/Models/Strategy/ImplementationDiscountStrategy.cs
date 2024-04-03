namespace Sofka.Piguinera.Cotizacion.Models.Strategy
{
    public class SeniorityOneTwoDiscountStrategy : IDiscountStrategy
    {
        public bool CanApply(int seniority) => seniority >= 1 && seniority <= 2;
        public float Apply() => 0.12f;
    }

    public class SeniorityMoreThanTwoDiscountStrategy : IDiscountStrategy
    {
        public bool CanApply(int seniority) => seniority > 2;
        public float Apply() => 0.17f;
    }
}

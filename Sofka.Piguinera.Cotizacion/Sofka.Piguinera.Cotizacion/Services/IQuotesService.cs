﻿using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

        String TotalPricePurchese(BaseBookDTO payload);

        String TotalPricePurcheses(List<BaseBookDTO> payload);

        string BooksBudget (BookWithBudgetDTO payload);

    }
}

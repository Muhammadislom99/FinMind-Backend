namespace FinMind.Application.Contract.Loans.Responses;

public record LoanResponse
{
    public Guid Id { get; init; } // Уникальный идентификатор займа
    public Guid AccountId { get; init; }
    public string Name { get; init; } // Название кредита (например, "Ипотека", "Кредит на авто")
    public string? Description { get; init; } // Дополнительное описание (опционально)

    /// <summary>
    /// Основная сумма кредита
    /// </summary>
    public decimal PrincipalAmount { get; init; }

    /// <summary>
    /// Годовая процентная ставка
    /// </summary>
    public decimal InterestRate { get; init; }

    /// <summary>
    /// Ежемесячный платёж
    /// </summary>
    public decimal MonthlyPayment { get; init; }

    /// <summary>
    /// Сколько уже выплачено по кредиту
    /// </summary>
    public decimal TotalPaid { get; init; }

    /// <summary>
    /// Сколько ещё осталось выплатить (с процентами)
    /// </summary>
    public decimal RemainingBalance => PrincipalAmount - TotalPaid;
    public DateOnly? EndDate { get; init; }
}
namespace FinMind.Domain.Enums;

public enum LoanStatus
{
    Active=1,    // Активный
    PaidOff,       // Погашен
    Defaulted,     // Просрочен
    Refinanced     // Рефинансирован
}
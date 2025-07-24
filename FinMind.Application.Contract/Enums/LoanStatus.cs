namespace FinMind.Application.Contract.Enums;

public enum LoanStatus
{
    Active=1,    // Активный
    PaidOff,       // Погашен
    Defaulted,     // Просрочен
    Refinanced     // Рефинансирован
}
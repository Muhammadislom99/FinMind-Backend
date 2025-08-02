namespace FinMind.Application.Contract.Enums;

public enum TransactionType
{
    Expense=1, // Рассход
    Income , //Доход
    Transfer ,//Между счетами
    Repayment, //Погашение
    Saving, // Накопление
    DebtPayment
}
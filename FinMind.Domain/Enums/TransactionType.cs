namespace FinMind.Domain.Enums;

public enum TransactionType
{
    Expense = 1, // Рассход
    Income, //Доход
    Transfer, //Между счетами
    Repayment, //Погашение
    Saving //Накопление
}
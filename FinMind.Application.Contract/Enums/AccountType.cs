namespace FinMind.Application.Contract.Enums;

public enum AccountType
{
    Cash=1,           // Наличные
    MobileWallet,    // Мобильный кошелёк
    SavingsAccount,     // Сберегательный счет
    CreditCard,         // Кредитная карта
    DebitCard,          // Дебетовая карта
    Investment,         // Инвестиционный счет
    Loan,               // Кредитный счет
    Goal,                   // Цель (Накопительный счет для накоплении определенной суммы денег)
}
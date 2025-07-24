using System.Transactions;
using FinMind.Domain.Enums;

namespace FinMind.Domain.Entities;

public class Account : BaseEntityWithNameDescription
{
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "TJK";
    public bool IsActive { get; set; } = true;

    public bool CheckingAccount { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    
}
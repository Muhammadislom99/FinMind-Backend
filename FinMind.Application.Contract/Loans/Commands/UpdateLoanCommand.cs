using FinMind.Application.Contract.Enums;
using FinMind.Application.Contract.Loans.Responses;

namespace FinMind.Application.Contract.Loans.Commands;

public record UpdateLoanCommand : IRequest<LoanResponse>
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public DateOnly EndDate { get; init; }
}

public class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
{
    public UpdateLoanCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
    }
}
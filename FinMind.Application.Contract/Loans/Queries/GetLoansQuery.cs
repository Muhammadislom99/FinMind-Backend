using FinMind.Application.Contract.Loans.Responses;

namespace FinMind.Application.Contract.Loans.Queries;

public record GetLoansQuery : IRequest<List<LoanResponse>>;
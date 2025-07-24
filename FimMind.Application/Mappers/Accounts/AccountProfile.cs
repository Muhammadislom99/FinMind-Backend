using AutoMapper;
using FinMind.Application.Contract.Accounts;
using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Application.Contract.Accounts.Responses;
using FinMind.Domain.Entities;

namespace FimMind.Application.Mappers.Accounts;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<Account, AccountResponse>();
    }
}
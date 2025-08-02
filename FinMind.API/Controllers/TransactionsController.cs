using FinMind.Application.Contract.Enums;
using FinMind.Application.Contract.Transactions.Expenses.Commands;
using FinMind.Application.Contract.Transactions.Incomes.Commands;
using FinMind.Application.Contract.Transactions.Queries;
using FinMind.Application.Contract.Transactions.Repayments.Commands;
using FinMind.Application.Contract.Transactions.Responses;
using FinMind.Application.Contract.Transactions.Tags.Commands;
using FinMind.Application.Contract.Transactions.Transfer.Commands;
using Microsoft.AspNetCore.Mvc;

namespace FinMind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ISender mediator) : ControllerBase
    {
        [HttpPost("trasfer")]
        public async Task<IActionResult> CreateTransfer([FromBody] CreateTransferTransactionCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("expense")]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseTransactionCommand transactionCommand)
        {
            return Ok(await mediator.Send(transactionCommand));
        }

        [HttpPost("income")]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeTransactionCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPost("repayment")]
        public async Task<IActionResult> CreateRepayment([FromBody] CreateRepaymentTransactionCommand command)
        {
            return Ok(await mediator.Send(command));
        }


        [HttpPost("/AddTags")]
        public async Task<IActionResult> AddTags([FromBody] AddTagsTransactionCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] GetTransactionsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpGet("GetAllTransactions2")]
        public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactions2(
            [FromQuery] GetTransactionsQuery query)
        {
            return Ok(await mediator.Send(query));
        }
    }
}
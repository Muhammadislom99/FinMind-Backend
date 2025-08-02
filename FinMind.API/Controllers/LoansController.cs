using FinMind.Application.Contract.Loans.Commands;
using FinMind.Application.Contract.Loans.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FinMind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController(ISender mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLoan([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new DeleteLoanCommand { Id = id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans()
        {
            return Ok(await mediator.Send(new GetLoansQuery()));
        }
    }
}
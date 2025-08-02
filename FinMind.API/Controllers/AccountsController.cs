using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Application.Contract.Accounts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FinMind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(ISender mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAccountCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAccountCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            return Ok(await mediator.Send(new GetAccountsQuery()));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new DeleteAccountCommand { Id = id }));
        }

        [HttpPut("SetCheckingAccount/{id:guid}")]
        public async Task<IActionResult> SetCheckingAccountAsync([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new SetCheckingAccountCommand() { AccountId = id }));
        }
    }
}
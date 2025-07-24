using FinMind.Application.Contract.Categories.Commands;
using FinMind.Application.Contract.Categories.Queries;
using FinMind.Application.Contract.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinMind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ISender mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet("Expenses")]
        public async Task<IActionResult> GetExpenseCategories()
        {
            return Ok(await mediator.Send(new GetCategoriesQuery(CategoryType.Expense)));
        }

        [HttpGet("Incomes")]
        public async Task<IActionResult> GetIncomesCategories()
        {
            return Ok(await mediator.Send(new GetCategoriesQuery(CategoryType.Income)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateCategoryCommand command)
        {
            command.Id = id;
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new DeleteCategoryCommand() { Id = id }));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetCategoryByIdQuery() { Id = id }));
        }
    }
}
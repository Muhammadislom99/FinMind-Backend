using Microsoft.AspNetCore.Mvc.Filters;
using FimMind.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinMind.API.Filters;

public class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = context switch
        {
            { Exception: NotFoundException } => HandleNotFoundException(context),
            { Exception: ExistException } => HandleExistException(context),
            { Exception: ValidationException } => HandleValidationException(context),
            { Exception: TransactionsException } => HandleBadRequestExeption(context),
            { Exception: HierarchyException } => HandleHierarchyException(context),
            _ => HandleUnknownException(context)
        };
        base.OnException(context);
    }

    private bool HandleHierarchyException(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "Invalid category hierarchy",
            Detail = exception.Message
        };
        context.Result = new ObjectResult(details);
        return true;
    }


    private bool HandleBadRequestExeption(ExceptionContext context)
    {
        var exception = context.Exception;
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Detail = exception.Message
        };
        context.Result = new ObjectResult(details);
        return true;
    }

    private bool HandleExistException(ExceptionContext context)
    {
        var exception = context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details);

        return true;
    }

    private bool HandleNotFoundException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = context.Exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        return true;
    }

    private bool HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        ValidationProblemDetails details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        return true;
    }

    public bool HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(details);
        logger.LogError(context.Exception, nameof(HandleUnknownException));

        return true;
    }
}
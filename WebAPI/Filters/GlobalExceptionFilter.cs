using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

	public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		_logger.LogError(context.Exception, "an unhandled exception occured");
		context.Result = new ObjectResult("an error occured while processing your request")
		{ 
			StatusCode = StatusCodes.Status500InternalServerError
		};
	}
}

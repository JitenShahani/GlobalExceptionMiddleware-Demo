namespace GlobalExceptionHandler.ExceptionHandlers;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
	private readonly IProblemDetailsService _problemDetailsService;
	private readonly ILogger<ValidationExceptionHandler> _logger;

	public ValidationExceptionHandler (IProblemDetailsService problemDetailsService, ILogger<ValidationExceptionHandler> logger)
		=> (_problemDetailsService, _logger) = (problemDetailsService, logger);

	public async ValueTask<bool> TryHandleAsync (HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		if (exception is not ValidationException validationException)
			return false;

		_logger.LogError (exception, exception.Message);

		httpContext.Response.StatusCode = exception switch
		{
			ValidationException _ => StatusCodes.Status400BadRequest,
			ApplicationException _ => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		var modelState = new ModelStateDictionary ();

		foreach (var error in validationException.Errors)
			modelState.AddModelError (error.PropertyName, error.ErrorMessage);

		var context = new ProblemDetailsContext
		{
			HttpContext = httpContext,
			Exception = exception,
			ProblemDetails = httpContext
				.RequestServices
				.GetRequiredService<ProblemDetailsFactory> ()
				.CreateValidationProblemDetails (httpContext, modelState, httpContext.Response.StatusCode)
		};

		context.ProblemDetails.Extensions["timestamp"] = DateTime.Now;

		return await _problemDetailsService.TryWriteAsync (context);
	}
}
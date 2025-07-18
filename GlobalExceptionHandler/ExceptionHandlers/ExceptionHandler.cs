namespace GlobalExceptionHandler.ExceptionHandlers;

public sealed class ExceptionHandler : IExceptionHandler
{
	private readonly ILogger<ExceptionHandler> _logger;

	public ExceptionHandler (ILogger<ExceptionHandler> logger)
		=> _logger = logger;

	public async ValueTask<bool> TryHandleAsync (HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError (exception, exception.Message);

		httpContext.Response.StatusCode = exception switch
		{
			ApplicationException _ => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory> ();
		var problemDetails = problemDetailsFactory.CreateProblemDetails (httpContext, httpContext.Response.StatusCode);

		Dictionary<string, object?> error = new ()
		{
			{ "message", exception.Message }
		};

		problemDetails.Extensions["errors"] = error;
		problemDetails.Extensions["timestamp"] = DateTime.Now;

		await httpContext.Response.WriteAsJsonAsync (problemDetails, cancellationToken);

		return true;
	}
}
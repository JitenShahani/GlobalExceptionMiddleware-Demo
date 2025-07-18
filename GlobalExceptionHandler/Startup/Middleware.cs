namespace GlobalExceptionHandler.Startup;

public static class Middleware
{
	public static void ConfigurePipeline (this WebApplication app)
	{
		// Activates the global exception-handling middleware.
		// Routes unhandled exceptions to registered IExceptionHandler implementations.
		app.UseExceptionHandler ();

		// Returns a ProblemDetails response for (empty) non-successful responses
		// such as HTTP status codes 400, 404, 409 when the body has not been written.
		app.UseStatusCodePages ();

		if (app.Environment.IsDevelopment ())
		{
			app.MapOpenApi ();
		}
	}
}
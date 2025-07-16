namespace GlobalExceptionHandler.Startup;

public static class Middleware
{
	public static void ConfigurePipeline (this WebApplication app)
	{
		// This middleware activates the global error handler.
		// It tells Asp Net Core to route any exceptions to our exception handler.
		app.UseExceptionHandler ();

		// Returns the Problem Details response for (empty) non-successful responses
		app.UseStatusCodePages ();

		if (app.Environment.IsDevelopment ())
		{
			app.MapOpenApi ();
		}
	}
}
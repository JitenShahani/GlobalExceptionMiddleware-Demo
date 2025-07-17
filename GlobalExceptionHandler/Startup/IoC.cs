namespace GlobalExceptionHandler.Startup;

public static class IoC
{
	public static void ConfigureIoCContainer (this WebApplicationBuilder builder)
	{
		// Register Validation Exception Handler
		builder.Services.AddExceptionHandler<ValidationExceptionHandler> ();

		// Register Generic Global Exception Handler
		builder.Services.AddExceptionHandler<ExceptionHandler> ();

		// Register Fluent Validators
		builder.Services.AddValidatorsFromAssemblyContaining<Program> ();

		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi ();

		// Configure JsonOptions
		builder.Services.Configure<JsonOptions> (options =>
		{
			// Configure JSON serializer to ignore null values during serialization
			options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

			// Configure JSON serializer to use Pascal case for property names during serialization
			options.JsonSerializerOptions.PropertyNamingPolicy = null;

			// Configure JSON serializer to use Pascal case for key's name during serialization
			options.JsonSerializerOptions.DictionaryKeyPolicy = null;

			// Ensure JSON property names are not case-sensitive during deserialization
			options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

			// Prevent serialization issues caused by cyclic relationships in EF Core entities
			options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

			// Ensure the JSON output is consistently formatted for readability.
			// Not to be used in Production as the response message size could be large
			// options.JsonSerializerOptions.WriteIndented = true;
		});

		// Register controllers to access ProblemDetailsFactory
		builder.Services.AddControllers ();

		// Configure ProblemDetails
		builder.Services.AddProblemDetails (options =>
		{
			options.CustomizeProblemDetails = context =>
			{
				context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

				context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
			};
		});
	}
}
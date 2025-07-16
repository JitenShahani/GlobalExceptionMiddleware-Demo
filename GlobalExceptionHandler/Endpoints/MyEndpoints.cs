namespace GlobalExceptionHandler.Endpoints;

public sealed class MyEndpoints
{
	public void MapMyEndpoints (WebApplication app)
	{
		app.MapGet ("/exception", () =>
		{
			throw new Exception ("Something went wrong in the endpoint.");
		});

		app.MapGet ("/divideByZero", (int num2 = 0) =>
		{
			var num1 = 10;

			var result = num1 / num2;
			return TypedResults.Ok (result);
		});

		app.MapPost ("/validationException", (
			[FromBody] Person request,
			[FromServices] IValidator<Person> validator) =>
		{
			validator.ValidateAndThrow (request);

			return TypedResults.Ok (request);
		});
	}
}
namespace GlobalExceptionHandler.Validations;

public class PersonValidator : AbstractValidator<Person>
{
	public PersonValidator ()
	{
		RuleFor (p => p.FirstName)
			.NotEmpty ()
			.WithMessage ("First name is required");

		RuleFor (p => p.LastName)
			.NotEmpty ()
			.WithMessage ("Last name is required");

		RuleFor (p => p.EMail)
			.NotEmpty ()
			.WithMessage ("E-mail is required")
			.EmailAddress ()
			.WithMessage ("A valid email address is required");

		RuleFor (p => p.MobileNo)
			.NotEmpty ()
			.WithMessage ("Mobile No is required")
			.Matches (@"^\+91\s[0-9]{5}\s[0-9]{5}$")
			.WithMessage ("Mobile number must be in the format '+91 99999 99999'.");
	}
}
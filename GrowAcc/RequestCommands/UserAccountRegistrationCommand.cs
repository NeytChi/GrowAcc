using FluentValidation;
using GrowAcc.RequestCommands;

namespace GrowAcc.RequestCommands
{
    public class UserAccountRegistrationCommand
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
public class UserAccountValidator : AbstractValidator<UserAccountRegistrationCommand>
{
    public UserAccountValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be a valid international format.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");
       
        //RuleFor(x => x.Password)
            //.Must(password => BeAValidPassword(password))
            //.WithMessage("Password must meet complexity requirements.");
    }
}

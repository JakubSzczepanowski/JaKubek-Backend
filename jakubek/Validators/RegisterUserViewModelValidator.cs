using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using jakubek.Entities;
using jakubek.Models;

namespace jakubek.Validators
{
    public class RegisterUserViewModelValidator : AbstractValidator<RegisterUserViewModel>
    {
        public RegisterUserViewModelValidator(BaseContext baseContext)
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .MinimumLength(3)
                .Custom((value,context) =>
                {
                    bool loginInUse = baseContext.Users.Any(u => u.Login == value);
                    if (loginInUse)
                        context.AddFailure("Login", "Ten login jest już używany");
                });

            RuleFor(c => c.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password);


        }
    }
}

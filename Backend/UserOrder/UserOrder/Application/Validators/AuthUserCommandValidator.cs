using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Application.Validators
{
    public class AuthUserCommandValidator :AbstractValidator<AuthUserCommand>
    {
        public AuthUserCommandValidator() {
            RuleFor(cmd => cmd.UserName)
                .NotEmpty().WithMessage("Username is required")
                .EmailAddress().WithMessage("Username is not valid email address.");
            RuleFor(cmd => cmd.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}

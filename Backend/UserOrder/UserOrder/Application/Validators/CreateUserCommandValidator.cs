using Domain.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Commands;

namespace UserOrder.Application.Validators
{
    public class CreateUserCommandValidator :AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator() {
            RuleFor(cmd => cmd.Email).EmailAddress().WithMessage("Not a valid email address");
            RuleFor(cmd => cmd.FirstName).NotEmpty().WithMessage("Firstname is required");
            RuleFor(cmd => cmd.LastName).NotEmpty().WithMessage("Lastname is required");
            RuleFor(cmd => cmd.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(cmd => cmd.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}

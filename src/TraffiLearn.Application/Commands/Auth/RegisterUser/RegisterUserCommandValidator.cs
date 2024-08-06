﻿using FluentValidation;
using TraffiLearn.Application.CustomValidators;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.RegisterUser
{
    internal sealed class RegisterUserCommandValidator 
        : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());

            RuleFor(x => x.Username)
                .MaximumLength(Username.MaxLength)
                .NotEmpty();
        }
    }
}

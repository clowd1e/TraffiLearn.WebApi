﻿using FluentValidation;
using TraffiLearn.Application.Validators;
using TraffiLearn.Domain.Routes.RouteDescriptions;
using TraffiLearn.Domain.Routes.RouteNumbers;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Create
{
    internal sealed class CreateRouteCommandValidator
        : AbstractValidator<CreateRouteCommand>
    {
        public CreateRouteCommandValidator()
        {
            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();

            RuleFor(x => x.RouteNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(RouteNumber.MinValue);

            RuleFor(x => x.Description)
                .MaximumLength(RouteDescription.MaxLength)
                .When(x => x.Description is not null);

            RuleFor(x => x.Image)
                .NotEmpty();

            RuleFor(x => x.Image)
               .SetValidator(new ImageValidator())
               .When(x => x.Image is not null);
        }
    }
}

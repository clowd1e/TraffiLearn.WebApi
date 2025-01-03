﻿using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Routes.RouteDescriptions
{
    public static class RouteDescriptionErrors
    {
        public static readonly Error EmptyText =
            Error.Validation(
                code: "RouteDescription.EmptyText",
                description: "Route description cannot be empty.");

        public static Error TooLongText(int allowedLength) =>
            Error.Validation(
                code: "RouteDescription.TooLongText",
                description: $"Route description text must not exceed {allowedLength} characters.");
    }
}

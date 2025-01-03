﻿using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions
{
    public static class PlanDescriptionErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "PlanDescription.Empty",
                description: "Plan description cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "PlanDescription.TooLong",
                description: $"Plan description exceeds {maxLength} characters.");
    }
}

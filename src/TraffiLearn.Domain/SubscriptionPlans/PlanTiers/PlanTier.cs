﻿using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.PlanTiers
{
    public sealed class PlanTier : ValueObject
    {
        public const int MaxLength = 50;

        private PlanTier(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PlanTier> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<PlanTier>(PlanTierErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<PlanTier>(PlanTierErrors.TooLong(
                    MaxLength));
            }

            return new PlanTier(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}

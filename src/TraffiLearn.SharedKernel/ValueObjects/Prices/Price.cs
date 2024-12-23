﻿using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.SharedKernel.ValueObjects.Prices
{
    public sealed class Price : ValueObject
    {
        private Price(
            decimal amount,
            Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }

        public Currency Currency { get; }

        public static Result<Price> Create(
            decimal amount,
            Currency currency)
        {
            if (amount < 0)
            {
                return Result.Failure<Price>(PriceErrors.NegativeAmount);
            }

            return new Price(amount, currency);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}

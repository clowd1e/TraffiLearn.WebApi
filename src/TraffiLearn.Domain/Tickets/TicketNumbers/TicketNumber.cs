﻿using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Tickets.TicketNumbers
{
    public sealed class TicketNumber : ValueObject
    {
        public const int MinValue = 1;

        private TicketNumber(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static Result<TicketNumber> Create(int value)
        {
            if (value < MinValue)
            {
                return Result.Failure<TicketNumber>(
                    TicketNumberErrors.TooSmall(minValue: MinValue));
            }

            return new TicketNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}

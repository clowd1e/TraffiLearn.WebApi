﻿using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.Delete
{
    public sealed record DeleteTopicCommand(
        Guid? TopicId) : IRequest<Result>;
}

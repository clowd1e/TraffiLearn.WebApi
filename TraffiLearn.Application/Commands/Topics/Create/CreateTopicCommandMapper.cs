﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    internal sealed class CreateTopicCommandMapper : Mapper<CreateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(CreateTopicCommand source)
        {
            var topicId = new TopicId(Guid.NewGuid());

            return Topic.Create(
                id: topicId,
                number: TopicNumber.Create(source.TopicNumber.Value),
                title: TopicTitle.Create(source.Title));
        }
    }
}

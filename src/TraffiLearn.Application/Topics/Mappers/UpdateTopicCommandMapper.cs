﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Topics.Commands.Update;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Mappers
{
    internal sealed class UpdateTopicCommandMapper
        : Mapper<UpdateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(UpdateTopicCommand source)
        {
            Result<TopicNumber> topicNumberResult = TopicNumber.Create(source.TopicNumber.Value);

            if (topicNumberResult.IsFailure)
            {
                return Result.Failure<Topic>(topicNumberResult.Error);
            }

            Result<TopicTitle> topicTitleResult = TopicTitle.Create(source.Title);

            if (topicTitleResult.IsFailure)
            {
                return Result.Failure<Topic>(topicTitleResult.Error);
            }

            TopicId topicId = new(Guid.NewGuid());

            return Topic.Create(
                topicId: topicId,
                number: topicNumberResult.Value,
                title: topicTitleResult.Value, 
                imageUri: null);
        }
    }
}

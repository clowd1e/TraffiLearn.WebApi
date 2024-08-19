﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Topics;

namespace TraffiLearn.Application.Topics.Mappers
{
    internal sealed class TopicToTopicResponseMapper : Mapper<Topic, TopicResponse>
    {
        public override TopicResponse Map(Topic source)
        {
            return new TopicResponse(
                TopicId: source.Id.Value,
                TopicNumber: source.Number.Value,
                Title: source.Title.Value);
        }
    }
}
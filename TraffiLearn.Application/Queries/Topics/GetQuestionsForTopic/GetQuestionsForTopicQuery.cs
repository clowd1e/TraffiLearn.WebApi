﻿using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic
{
    public sealed record GetQuestionsForTopicQuery(
        Guid? TopicId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}

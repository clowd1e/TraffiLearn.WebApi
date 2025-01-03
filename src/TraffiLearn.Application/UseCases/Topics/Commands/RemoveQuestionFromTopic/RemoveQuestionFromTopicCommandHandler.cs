﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Commands.RemoveQuestionFromTopic
{
    internal sealed class RemoveQuestionFromTopicCommandHandler
        : IRequestHandler<RemoveQuestionFromTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionFromTopicCommandHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveQuestionFromTopicCommand request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdWithQuestionsAsync(
                topicId: new TopicId(request.TopicId),
                cancellationToken);

            if (topic is null)
            {
                return TicketErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId),
                cancellationToken);

            if (question is null)
            {
                return TicketErrors.QuestionNotFound;
            }

            var questionRemoveResult = topic.RemoveQuestion(question);

            if (questionRemoveResult.IsFailure)
            {
                return questionRemoveResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

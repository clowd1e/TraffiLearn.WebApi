﻿using MediatR;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetAIQuestionExplanation
{
    internal sealed class GetAIQuestionExplanationQueryHandler
        : IRequestHandler<GetAIQuestionExplanationQuery,
            Result<AITextResponse>>
    {
        private readonly IAIService _aiService;
        private readonly IQuestionRepository _questionRepository;

        public GetAIQuestionExplanationQueryHandler(
            IAIService aiService,
            IQuestionRepository questionRepository)
        {
            _aiService = aiService;
            _questionRepository = questionRepository;
        }

        public async Task<Result<AITextResponse>> Handle(
            GetAIQuestionExplanationQuery request,
            CancellationToken cancellationToken)
        {
            QuestionId questionId = new(request.QuestionId);

            var question = await _questionRepository.GetByIdAsync(
                questionId,
                cancellationToken);

            if (question is null)
            {
                return Result.Failure<AITextResponse>(
                    QuestionErrors.NotFound);
            }

            if (question.ImageUri is not null)
            {
                return Result.Failure<AITextResponse>(
                    QuestionErrors.CantGenerateExplanationIfQuestionHasImage);
            }

            var explanation = await _aiService.SendTextQueryAsync(
                ConvertQuestionToRequestContent(question),
                cancellationToken);

            return Result.Success(explanation);
        }

        private static AITextRequest ConvertQuestionToRequestContent(Question question)
        {
            var requestContent = $"Explain why the answer given as the correct one is correct in the question about traffic rules. " +
                $"Do not make assumptions about the country of origin of the question:\n" +
                $"Question: \"{question.Content.Value}\";\n" +
                $"Answers: {string.Join(", ",
                    question.Answers.Select(a =>
                        $"\"{a.Text}\"")
                    )};\n" +
                $"Correct answer: \"{question.Answers.Single(a => a.IsCorrect).Text}\"";

            var result = new AITextRequest(requestContent);

            return result;
        }
    }
}

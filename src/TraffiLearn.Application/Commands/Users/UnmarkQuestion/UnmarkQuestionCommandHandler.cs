﻿using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Commands.Users.MarkQuestion;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Users.UnmarkQuestion
{
    internal sealed class UnmarkQuestionCommandHandler
        : IRequestHandler<UnmarkQuestionCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MarkQuestionCommandHandler> _logger;

        public UnmarkQuestionCommandHandler(
            IUserManagementService userManagementService,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork,
            ILogger<MarkQuestionCommandHandler> logger)
        {
            _userManagementService = userManagementService;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            UnmarkQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var userResult = await GetCaller(cancellationToken);

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var user = userResult.Value;

            var markResult = user.UnmarkQuestion(question);

            if (markResult.IsFailure)
            {
                return markResult.Error;
            }

            await _userRepository.UpdateAsync(user);
            await _questionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully unmarked question. User's username: {username}", user.Username.Value);

            return Result.Success();
        }

        private async Task<Result<User>> GetCaller(
            CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.MarkedQuestions
                ]);
        }
    }
}

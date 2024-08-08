﻿using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetMarkedQuestions
{
    internal sealed class GetMarkedQuestionsQueryHandler
        : IRequestHandler<GetMarkedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetMarkedQuestionsQueryHandler> _logger;

        public GetMarkedQuestionsQueryHandler(
            IAuthService<ApplicationUser> authService,
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetMarkedQuestionsQueryHandler> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetMarkedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userIdResult = _authService.GetAuthenticatedUserId();

            if (userIdResult.IsFailure)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(userIdResult.Error);
            }

            var user = await _userRepository.GetByIdAsync(
                userId: new UserId(userIdResult.Value),
                cancellationToken,
                includeExpressions: user => user.MarkedQuestions);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return Result.Failure<IEnumerable<QuestionResponse>>(InternalErrors.AuthenticatedUserNotFound);
            }

            _logger.LogInformation("Succesfully fetched user's marked questions. Questions fetched: {0}", user.MarkedQuestions.Count);

            return Result.Success(_questionMapper.Map(user.MarkedQuestions));
        }
    }
}

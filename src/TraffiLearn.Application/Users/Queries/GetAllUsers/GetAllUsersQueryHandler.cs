﻿using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<User, UserResponse> _userMapper;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            Mapper<User, UserResponse> userMapper,
            ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var callerId = _userContextService.GetAuthenticatedUserId();

            var caller = await _userRepository.GetByIdAsync(
                new UserId(callerId),
                cancellationToken);

            if (caller is null)
            {
                throw new AuthorizationFailureException();
            }

            _logger.LogInformation(
                "Succesfully fetched caller with the id: {callerId}", callerId);

            if (CallerIsNotEligibleToGetAllUsers(caller))
            {
                _logger.LogInformation(
                    "Caller with the role {role} tried to fetch all users, " +
                    "but does not have enough rights. Returning error.", caller.Role);

                return Result.Failure<IEnumerable<UserResponse>>(
                    UserErrors.NotAllowedToPerformAction);
            }

            var allUsers = await _userRepository.GetAllAsync(
                cancellationToken);

            _logger.LogInformation("Succesfully fetched all users for the caller. Returning result.");

            return Result.Success(_userMapper.Map(allUsers));
        }

        private static bool CallerIsNotEligibleToGetAllUsers(User caller)
        {
            return caller.Role < Role.Admin;
        }
    }
}

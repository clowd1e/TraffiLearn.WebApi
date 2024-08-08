﻿using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.RegisterAdmin
{
    internal sealed class RegisterAdminCommandHandler
        : IRequestHandler<RegisterAdminCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly Mapper<RegisterAdminCommand, Result<User>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthSettings _authSettings;
        private readonly ILogger<RegisterAdminCommandHandler> _logger;

        public RegisterAdminCommandHandler(
            IUserRepository userRepository,
            IAuthService<ApplicationUser> authService,
            Mapper<RegisterAdminCommand, Result<User>> commandMapper,
            IUnitOfWork unitOfWork,
            IOptions<AuthSettings> authSettings,
            ILogger<RegisterAdminCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
            _authSettings = authSettings.Value;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterAdminCommand request,
            CancellationToken cancellationToken)
        {
            Result<Guid> creatorIdResult = _authService.GetAuthenticatedUserId();

            if (creatorIdResult.IsFailure)
            {
                return creatorIdResult.Error;
            }

            UserId creatorId = new(creatorIdResult.Value);

            var creator = await _userRepository.GetByIdAsync(
                creatorId,
                cancellationToken);

            if (creator is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

            if (creator.Role < _authSettings.MinimumAllowedRoleToCreateAdminAccounts)
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newAdmin = mappingResult.Value;

            var existsSameUser = await _userRepository.ExistsAsync(
                newAdmin.Username,
                newAdmin.Email,
                cancellationToken);

            if (existsSameUser)
            {
                return UserErrors.AlreadyRegistered;
            }

            // Transaction is required due to features of UserManager.
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.AddAsync(
                    newAdmin,
                    cancellationToken);

                var identityUser = CreateIdentityUser(newAdmin);

                var addResult = await _authService.AddIdentityUser(
                    identityUser,
                    request.Password);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }

                var roleAssignmentResult = await _authService.AssignRoleToUser(
                    identityUser,
                    role: newAdmin.Role);

                if (roleAssignmentResult.IsFailure)
                {
                    return roleAssignmentResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Successfully created a new admin user with {Email} email.", identityUser.Email);

                transaction.Complete();
            }

            return Result.Success();
        }

        private ApplicationUser CreateIdentityUser(User user)
        {
            return new ApplicationUser()
            {
                Id = user.Id.ToString(),
                Email = user.Email.Value,
                UserName = user.Username.Value
            };
        }
    }
}

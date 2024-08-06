﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RegisterUser
{
    internal sealed class RegisterUserCommandHandler
        : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Mapper<RegisterUserCommand, Result<User>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IAuthService<ApplicationUser> authService,
            Mapper<RegisterUserCommand, Result<User>> commandMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userManager = userManager;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newUser = mappingResult.Value;

            var existsSameUser = await _userRepository
                .ExistsAsync(
                    newUser.Username, 
                    newUser.Email,
                    cancellationToken);

            if (existsSameUser)
            {
                return UserErrors.AlreadyRegistered;
            }

            await _userRepository.AddAsync(
                newUser,
                cancellationToken);

            var newIdentityUser = CreateIdentityUser(newUser);

            var addResult = await AddIdentityUser(
                newIdentityUser,
                password: request.Password);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            var roleAssignmentResult = await _authService.AssignRoleToUser(
                newIdentityUser,
                role: Role.RegularUser);

            if (roleAssignmentResult.IsFailure)
            {
                return roleAssignmentResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully created a new user with {Email} email.", newUser.Email);

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

        private async Task<Result> AddIdentityUser(
            ApplicationUser identityUser,
            string password)
        {
            var result = await _userManager.CreateAsync(
                identityUser,
                password);

            if (!result.Succeeded)
            {
                _logger.LogCritical("Failed to create identity user.");

                return Error.InternalFailure();
            }

            return Result.Success();
        }
    }
}

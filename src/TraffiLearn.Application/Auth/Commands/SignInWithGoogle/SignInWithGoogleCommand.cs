﻿using MediatR;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.SignInWithGoogle
{
    public sealed record SignInWithGoogleCommand(
        string? GoogleIdToken,
        string? FirstTimeSignInPassword) : IRequest<Result<LoginResponse>>;
}

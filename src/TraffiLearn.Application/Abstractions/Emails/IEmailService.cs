﻿using TraffiLearn.Application.UseCases.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailService
    {
        Task PublishConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser);

        Task PublishChangeEmailMessageAsync(
            string newEmail,
            string userId,
            ApplicationUser identityUser);

        Task PublishRecoverPasswordEmail(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser);
    }
}

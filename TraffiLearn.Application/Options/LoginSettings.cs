﻿namespace TraffiLearn.Application.Options
{
    public sealed class LoginSettings
    {
        public const string SectionName = nameof(LoginSettings);

        public bool IsPersistent = true;

        public bool LockoutOnFailure = false;
    }
}

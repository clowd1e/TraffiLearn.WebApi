﻿using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.Errors.Directories
{
    public static class DirectoryNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "DirectoryName.Empty",
                description: "Directory name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "DirectoryName.TooLong",
                description: $"Directory name must not exceed {allowedLength} characters.");
    }
}

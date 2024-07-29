﻿using FluentValidation;

namespace TraffiLearn.Application.Commands.Questions.RemoveTopicForQuestion
{
    internal sealed class RemoveTopicForQuestionCommandValidator : AbstractValidator<RemoveTopicForQuestionCommand>
    {
        public RemoveTopicForQuestionCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}

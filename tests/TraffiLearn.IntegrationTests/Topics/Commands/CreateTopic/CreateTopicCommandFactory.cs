﻿using TraffiLearn.Application.UseCases.Topics.Commands.Create;
using TraffiLearn.Domain.Topics.TopicTitles;

namespace TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandFactory
    {
        public List<CreateTopicCommand> GetInvalidCommands()
        {
            return [
                new CreateTopicCommand(1, string.Empty),
                new CreateTopicCommand(-1, "title"),
                new CreateTopicCommand(1, null!),
                new CreateTopicCommand(1, new string('1', TopicTitle.MaxLength + 1))
            ];
        }

        public CreateTopicCommand CreateValidCommand()
        {
            return new CreateTopicCommand(
                TopicNumber: 1,
                Title: "title");
        }
    }
}

﻿using FluentAssertions;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetTopicQuestions
{
    public sealed class GetTopicQuestionsTests : TopicIntegrationTest
    {
        public GetTopicQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetTopicQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendGetTopicQuestionsAsync(
                topicId: Guid.NewGuid(),
                sentWithRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendGetTopicQuestionsAsync(
                topicId: Guid.NewGuid(),
                sentWithRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfTopicContainsNoQuestions_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendGetTopicQuestionsAsync(
                topicId: topicId,
                sentWithRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfUserIsEligibleButTopicContainsNoQuestions_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsync(
                topicId,
                getWithRole: eligibleRole);

            topicQuestions.Should().NotBeNull();
            topicQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfValidCase_ShouldReturn200StatusCode(
           Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendGetTopicQuestionsAsync(
                topicId: topicId,
                sentWithRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfValidCase_ShouldReturnTopicQuestions(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsync(
                topicId,
                getWithRole: eligibleRole);

            topicQuestions.Should().HaveCount(1);
            topicQuestions.Single().Id.Should().Be(questionId);
        }
    }
}
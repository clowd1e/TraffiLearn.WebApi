﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetTicketQuestions
{
    internal sealed class GetTicketQuestionsQueryHandler
        : IRequestHandler<GetTicketQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetTicketQuestionsQueryHandler(
            ITicketRepository ticketRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _ticketRepository = ticketRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetTicketQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken,
                includeExpressions: ticket => ticket.Questions);

            if (ticket is null)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(TicketErrors.QuestionNotFound);
            }

            return Result.Success(_questionMapper.Map(ticket.Questions));
        }
    }
}

﻿using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Tickets
{
    public interface ITicketRepository : IRepositoryMarker
    {
        Task<Ticket?> GetByIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default);

        Task<Ticket?> GetByIdWithQuestionsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Ticket>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Ticket>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<Ticket?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Ticket ticket,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Ticket ticket);

        Task DeleteAsync(Ticket ticket);
    }
}

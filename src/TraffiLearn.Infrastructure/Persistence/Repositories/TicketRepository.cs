﻿using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(
            Ticket ticket,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Tickets.AddAsync(
                ticket,
                cancellationToken);
        }

        public Task DeleteAsync(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets.FindAsync(
                keyValues: [ticketId],
                cancellationToken) is not null;
        }

        public Task UpdateAsync(Ticket ticket)
        {
            _dbContext.Tickets.Update(ticket);

            return Task.CompletedTask;
        }

        public async Task<Ticket?> GetByIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets
                .FindAsync(
                    keyValues: [ticketId],
                    cancellationToken);
        }

        public async Task<Ticket?> GetByIdWithQuestionsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets
                .Where(t => t.Id == ticketId)
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets
                .Where(t => t.Questions.Any(q => q.Id == questionId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Tickets
                .ToListAsync(cancellationToken);
        }

        public Task<Ticket?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default)
        {
            var sql = """
                SELECT *
                FROM "{0}"
                ORDER BY RANDOM()
                LIMIT 1
            """;

            var formattedSql = string.Format(
               sql,
               nameof(ApplicationDbContext.Tickets));

            return _dbContext.Tickets
                .FromSqlRaw(formattedSql)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

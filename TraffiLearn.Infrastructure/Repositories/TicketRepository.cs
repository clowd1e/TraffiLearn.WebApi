﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _dbContext.Tickets.AddAsync(ticket);
        }

        public Task DeleteAsync(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await _dbContext.Tickets.FindAsync(id)) is not null;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _dbContext.Tickets.ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(
            Guid ticketId, 
            Expression<Func<Ticket, object>> includeExpression = null!)
        {
            if (includeExpression is null)
            {
                return await _dbContext.Tickets.FindAsync(ticketId);
            }

            IQueryable<Ticket> query = _dbContext.Tickets;

            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(q => q.Id == ticketId);
        }

        public Task UpdateAsync(Ticket ticket)
        {
            _dbContext.Tickets.Update(ticket);

            return Task.CompletedTask;
        }
    }
}

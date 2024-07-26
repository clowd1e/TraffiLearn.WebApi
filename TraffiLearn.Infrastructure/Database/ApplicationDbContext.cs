﻿using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Topic> Topics { get; set; }
    }
}

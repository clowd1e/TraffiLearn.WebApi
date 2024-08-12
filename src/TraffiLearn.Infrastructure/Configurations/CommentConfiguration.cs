﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Id).HasConversion(
                id => id.Value,
                value => new CommentId(value));

            builder.Property(c => c.Content)
                .HasMaxLength(CommentContent.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => CommentContent.Create(value).Value);

            builder
                .HasOne(c => c.Question)
                .WithMany(q => q.Comments);

            builder
                .HasOne(c => c.Creator)
                .WithMany(q => q.Comments);

            builder
                .HasMany(c => c.Replies)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.RootComment)
                .WithMany(c => c.Replies)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}

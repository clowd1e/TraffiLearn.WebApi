﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.Domain.Users.Usernames;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Id)
                .IsUnique();

            builder.Property(user => user.Id).HasConversion(
                 id => id.Value,
                 value => new UserId(value));

            builder.Property(user => user.Username)
                .HasMaxLength(Username.MaxLength)
                .HasConversion(
                    username => username.Value,
                    value => Username.Create(value).Value);

            builder.Property(user => user.Email)
                .HasMaxLength(Email.MaxLength)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value);

            builder
                .HasIndex(user => user.Email)
                .IsUnique();

            builder
                .HasIndex(user => user.Username)
                .IsUnique();

            builder
                .HasMany(user => user.Comments)
                .WithOne(c => c.Creator);

            builder
                .HasOne(user => user.SubscriptionPlan)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .Property(user => user.PlanExpiresOn)
                .IsRequired(false);

            builder
                .HasMany(user => user.MarkedQuestions)
                .WithMany(question => question.MarkedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsMarkedByUsers"));

            builder
                .HasMany(user => user.LikedQuestions)
                .WithMany(question => question.LikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsLikedByUsers"));

            builder
                .HasMany(user => user.DislikedQuestions)
                .WithMany(question => question.DislikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsDislikedByUsers"));

            builder
                .HasMany(user => user.LikedComments)
                .WithMany(comment => comment.LikedByUsers)
                .UsingEntity(join => join.ToTable("CommentsLikedByUsers"));

            builder
                .HasMany(user => user.DislikedComments)
                .WithMany(comment => comment.DislikedByUsers)
                .UsingEntity(join => join.ToTable("CommentsDislikedByUsers"));

            builder
                .HasMany(user => user.Transactions)
                .WithOne(t => t.User);
        }
    }
}

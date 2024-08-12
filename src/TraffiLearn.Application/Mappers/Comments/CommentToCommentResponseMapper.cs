﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Mapper.Comments
{
    internal sealed class CommentToCommentResponseMapper
        : Mapper<Comment, CommentResponse>
    {
        public override CommentResponse Map(Comment source)
        {
            bool hasReplies = source.Replies.Count > 0;

            return new CommentResponse(
                CommentId: source.Id.Value,
                AuthorUserId: source.Creator.Id.Value,
                AuthorUsername: source.Creator.Username.Value,
                Content: source.Content.Value,
                HasReplies: hasReplies);
        }
    }
}

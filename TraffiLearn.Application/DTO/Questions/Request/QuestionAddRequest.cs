﻿using System.ComponentModel.DataAnnotations;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed class QuestionAddRequest
    {
        [Required]
        [StringLength(500)]
        public string? Text { get; set; }

        [Required]
        public List<string>? PossibleAnswears { get; set; }

        [Required]
        public List<string>? CorrectAnswears { get; set; }

        [Required]
        public QuestionNumberDetails? NumberDetails { get; set; }

        //[Required]
        //public List<Guid>? CategoriesIds { get; set; }
    }
}

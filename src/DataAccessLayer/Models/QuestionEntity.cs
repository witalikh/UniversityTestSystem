// <copyright file="QuestionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Models;

public class QuestionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("TestEntity")]
    public int TestId { get; set; }

    [Required]
    public int InnerOrder { get; set; } = 0;

    [Required]
    public QuestionType Type { get; set; }

    [Required]
    public int Marks { get; set; } = 1;

    [Required]
    public string QuestionText { get; set; }

    public double? CorrectNumber { get; set; }

    public string? CorrectShortText { get; set; }

    // getters (foreign)
    public TestEntity TestEntity { get; set; }

    // getters (reverse)
    public ICollection<QuestionChoiceOptionEntity> ChoiceOptions { get; }

    public ICollection<AnswerEntity> Answers { get; }
}
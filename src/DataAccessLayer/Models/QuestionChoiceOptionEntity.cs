// <copyright file="QuestionChoiceOptionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

/// <summary>
/// Empty.
/// </summary>
public class QuestionChoiceOptionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("QuestionEntity")]
    public int QuestionId { get; set; }

    [Required]
    public int InnerOrder { get; set; } = 0;

    [Required]
    public string OptionText { get; set; }

    [Required]
    public bool IsCorrect { get; set; } = false;

    // getters (foreign)
    public QuestionEntity QuestionEntity { get; set; }

    public ICollection<AnswerChosenOptionEntity> AnswerChosenOptions { get; }
}
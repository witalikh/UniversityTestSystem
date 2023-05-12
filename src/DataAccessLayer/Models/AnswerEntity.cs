// <copyright file="AnswerEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

/// <summary>
/// Class for student answers
/// Base table for different question types.
/// </summary>
public class AnswerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // AnswerEntity is attached to some attempt
    [Required]
    [ForeignKey("AttemptEntity")]
    public Guid AttemptUuid { get; set; }

    public AttemptEntity? Attempt { get; set; }

    // The question of the testEntity, actually assigned question
    [Required]
    [ForeignKey("QuestionEntity")]
    public int QuestionId { get; set; }

    public QuestionEntity? Question { get; set; }

    // Marks obtained by the question answer
    // null = unassigned (e.g. open answers requiring manual checking)
    public int? Marks { get; set; }

    // getters (reverse)
    public ICollection<AnswerChosenOptionEntity>? ChosenOptions { get; }

    public double? NumericValue { get; set; }

    public string? ShortTextValue { get; set; }
}

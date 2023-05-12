// <copyright file="AnswerChosenOptionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

/// <summary>
/// Single or multiple choices answers
/// Attached to the question.
/// </summary>
public class AnswerChosenOptionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("AnswerEntity")]
    public int AnswerId { get; set; }

    // Represent option student actually chosen
    // every record in the table is selected answer options
    [Required]
    [ForeignKey("QuestionChoiceOptionEntity")]
    public int ChoiceOptionId { get; set; }

    // getters (foreign key)
    public AnswerEntity AnswerEntity { get; set; }

    public QuestionChoiceOptionEntity ChoiceOptionEntity { get; set; }

}

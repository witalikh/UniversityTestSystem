// <copyright file="AnswerChosenOptionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

/// <summary>
/// Single or multiple choices answers
/// Attached to the question.
/// </summary>
public class AnswerChosenOptionViewModel
{
    public int Id { get; set; }

    public int AnswerId { get; set; }

    public int ChoiceOptionId { get; set; }

    public AnswerChosenOptionViewModel()
    {
    }

    public AnswerChosenOptionViewModel(AnswerChosenOptionEntity entity)
    {
        this.Id = entity.Id;

        this.AnswerId = entity.AnswerId;

        this.ChoiceOptionId = entity.ChoiceOptionId;
    }
}


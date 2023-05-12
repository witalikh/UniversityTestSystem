// <copyright file="QuestionChoiceOptionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using DataAccessLayer.Models;
using System.ComponentModel;

namespace BusinessLayer.ViewModels;

/// <summary>
/// Empty.
/// </summary>
public class QuestionChoiceOptionViewModel
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    [DisplayName("Inner order")]
    public int InnerOrder { get; set; } = 0;

    [DisplayName("Option text")]
    public string OptionText { get; set; } = string.Empty;

    [DisplayName("Is correct?")]
    public bool IsCorrect { get; set; } = false;

    public QuestionChoiceOptionViewModel() {}

    public QuestionChoiceOptionViewModel(QuestionChoiceOptionEntity entity)
    {
        this.Id = entity.Id;

        this.QuestionId = entity.QuestionId;

        this.InnerOrder = entity.InnerOrder;

        this.OptionText = entity.OptionText;

        this.IsCorrect = entity.IsCorrect;
    }

    public static QuestionChoiceOptionEntity ToEntity(QuestionChoiceOptionViewModel viewModel)
    {
        var entity = new QuestionChoiceOptionEntity()
        {
            Id = viewModel.Id,
            QuestionId = viewModel.QuestionId,
            InnerOrder = viewModel.InnerOrder,
            OptionText = viewModel.OptionText,
            IsCorrect = viewModel.IsCorrect,
        };

        return entity;
    }
}
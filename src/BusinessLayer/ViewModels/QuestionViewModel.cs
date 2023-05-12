// <copyright file="QuestionEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

public class QuestionViewModel
{
    public int Id { get; set; }

    public int TestId { get; set; } = 0;

    [DisplayName("Inner order")]
    public int InnerOrder { get; set; } = 0;

    [DisplayName("Question Type")]
    public QuestionType Type { get; set; } = QuestionType.ShortText;

    [DisplayName("Marks per question")]
    public int Marks { get; set; } = 0;

    [DisplayName("Question text")]
    public string QuestionText { get; set; } = string.Empty;

    [DisplayName("Correct number (when type is numerical)")]
    public double? CorrectNumber { get; set; } = 0;

    [DisplayName("Correct text (when type is short text)")]
    public string CorrectShortText { get; set; } = string.Empty;

    [DisplayName("Options (when type is choices)")]
    public List<QuestionChoiceOptionViewModel> ChoiceOptions { get; set; } = new List<QuestionChoiceOptionViewModel>();

    public QuestionViewModel() {}

    public QuestionViewModel(QuestionEntity entity)
    {
        this.Id = entity.Id;

        this.TestId = entity.TestId;

        this.InnerOrder = entity.InnerOrder;

        this.Type = entity.Type;

        this.Marks = entity.Marks;

        this.QuestionText = entity.QuestionText;

        this.CorrectNumber = entity.CorrectNumber;

        this.CorrectShortText = entity.CorrectShortText;

        this.ChoiceOptions = entity.ChoiceOptions?
            .Select(x => new QuestionChoiceOptionViewModel(x))
            .ToList() ?? new List<QuestionChoiceOptionViewModel>();
    }

    public static QuestionEntity ToEntity(QuestionViewModel viewModel)
    {
        var entity = new QuestionEntity
        {
            Id = viewModel.Id,
            TestId = viewModel.TestId,
            Type = viewModel.Type,
            Marks = viewModel.Marks,
            InnerOrder = viewModel.InnerOrder,
            QuestionText = viewModel.QuestionText,
            CorrectNumber = viewModel.CorrectNumber,
            CorrectShortText = viewModel.CorrectShortText,
            // Choices = viewModel.Choices.Select(c => ChoiceMapper.ToEntity(c)).ToList()
        };

        return entity;
    }
}
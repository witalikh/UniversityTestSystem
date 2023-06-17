// <copyright file="AnswerEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

/// <summary>
/// Class for student answers
/// Base table for different question types.
/// </summary>
public class AnswerViewModel
{
    public int Id { get; set; }

    public Guid AttemptUuid { get; set; }

    public int QuestionId { get; set; }

    public int? Marks { get; set; }

    public double? NumericValue { get; set; }

    public string? ShortTextValue { get; set; }

    public List<AnswerChosenOptionViewModel>? ChosenOptions { get; set; } = new List<AnswerChosenOptionViewModel>();

    public AnswerViewModel()
    {
    }

    public AnswerViewModel(AnswerEntity entity)
    {
        this.Id = entity.Id;

        this.AttemptUuid = entity.AttemptUuid;

        this.QuestionId = entity.QuestionId;

        this.Marks = entity.Marks;

        this.NumericValue = entity.NumericValue;

        this.ShortTextValue = entity.ShortTextValue;


        if (entity.ChosenOptions != null)
        {
            this.ChosenOptions = entity.ChosenOptions.Select(co => new AnswerChosenOptionViewModel(co)).ToList();
        }
    }

    public static AnswerEntity ToEntity(AnswerViewModel viewModel)
    {
        AnswerEntity answerEntity = new AnswerEntity()
        {
            AttemptUuid = viewModel.AttemptUuid,
            QuestionId = viewModel.QuestionId,
            Marks = viewModel.Marks,
            NumericValue = viewModel.NumericValue,
            ShortTextValue = viewModel.ShortTextValue,
        };

        if (viewModel.ChosenOptions != null)
        {
            foreach (var optionData in viewModel.ChosenOptions)
            {
                answerEntity.ChosenOptions!.Add(AnswerChosenOptionViewModel.ToEntity(optionData));
            }
        }

        return answerEntity;
    }
}

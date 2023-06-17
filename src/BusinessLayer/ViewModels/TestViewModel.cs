// <copyright file="TestEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

public class TestViewModel
{
    public int? Id { get; set; }

    public int ClassroomId { get; set; }

    public string? OwnerId { get; set; }

    //[DisplayFormat(DataFormatString = "dd.MM.yyyy hh:mm", ApplyFormatInEditMode = true)]
    [DisplayName("Start date & time")]
    public DateTime? StartDateTime { get; set; } = DateTime.Now.AddDays(1);

    [DisplayName("End date & time")]
    public DateTime? EndDateTime { get; set; } = DateTime.Now.AddDays(1).AddHours(2);

    [DisplayName("Duration (in seconds)")]
    public int? DurationSeconds { get; set; } = 1 * 3600;

    [DisplayName("Attempts number")]
    public int? AttemptsAllowed { get; set; } = 1;

    [DisplayName("Total grade")]
    public int TotalGrade { get; set; } = 10;

    [DisplayName("Questions")]
    public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();

    public TestViewModel() {}

    public TestViewModel(TestEntity entity)
    {
        this.Id = entity.Id;

        this.ClassroomId = entity.ClassroomId;
        
        this.OwnerId = entity.ClassroomEntity.OwnerId;

        this.StartDateTime = entity.StartDateTime;

        this.EndDateTime = entity.EndDateTime;

        this.DurationSeconds = (int?)entity.DurationSeconds;

        this.AttemptsAllowed = (int?)entity.AttemptsAllowed;

        this.TotalGrade = (int)entity.TotalGrade;

        this.Questions = entity.Questions.Select(q => new QuestionViewModel(q)).ToList();
    }

    public static TestEntity ToEntity(TestViewModel viewModel)
    {
        if (viewModel.Id != null)
        {
            return new TestEntity
            {
                Id = viewModel.Id.Value,
                ClassroomId = viewModel.ClassroomId,
                StartDateTime = viewModel.StartDateTime,
                EndDateTime = viewModel.EndDateTime,
                DurationSeconds = viewModel.DurationSeconds,
                AttemptsAllowed = viewModel.AttemptsAllowed,
                TotalGrade = viewModel.TotalGrade,
                // Questions = viewModel.Questions.Select(q => QuestionMapper.ToEntity(q)).ToList()
            };
        }

        return new TestEntity
        {
            ClassroomId = viewModel.ClassroomId,
            StartDateTime = viewModel.StartDateTime,
            EndDateTime = viewModel.EndDateTime,
            DurationSeconds = viewModel.DurationSeconds,
            AttemptsAllowed = viewModel.AttemptsAllowed,
            TotalGrade = viewModel.TotalGrade,
            // Questions = viewModel.Questions.Select(q => QuestionMapper.ToEntity(q)).ToList()
        };
    }
}
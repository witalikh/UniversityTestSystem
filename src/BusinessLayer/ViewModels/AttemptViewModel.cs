// <copyright file="AttemptEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

public class AttemptViewModel
{
    public Guid uuid { get; set; }

    public int MemberId { get; set; }

    public int TestId { get; set; }

    public int AttemptNumber { get; set; }

    public int Grade { get; set; }

    public DateTime DateTimeStarted { get; set; }

    public DateTime DateTimeEnded { get; set; }

    public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();

    public AttemptViewModel() {}

    public AttemptViewModel(AttemptEntity entity)
    {
        this.uuid = entity.uuid;
        this.MemberId = entity.MemberId;
        this.TestId = entity.TestId;
        this.AttemptNumber = entity.AttemptNumber;
        this.Grade = entity.Grade;
        this.DateTimeStarted = entity.DateTimeStarted;
        this.DateTimeEnded = entity.DateTimeEnded;
        this.Answers = entity.Answers.Select(answer => new AnswerViewModel(answer)).ToList();
    }
}

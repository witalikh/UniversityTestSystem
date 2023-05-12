// <copyright file="TestEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class TestEntity
{
    [Key]
    public int Id { get; set; }

    // classroomEntity
    [Required]
    [ForeignKey("ClassroomEntity")]
    public int ClassroomId { get; set; }

    public ClassroomEntity ClassroomEntity { get; set; }

    // metadata
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // testEntity
    // time options
    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public int? DurationSeconds { get; set; }

    // testEntity
    public int? AttemptsAllowed { get; set; }

    [Required]
    public int TotalGrade { get; set; } = 0;

    // getters(reverse)
    public ICollection<QuestionEntity> Questions { get; }

    public ICollection<AttemptEntity> Attempts { get; }
}
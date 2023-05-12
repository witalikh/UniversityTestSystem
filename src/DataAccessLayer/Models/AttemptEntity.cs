// <copyright file="AttemptEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class AttemptEntity
{
    [Key]
    public Guid uuid { get; set; }

    [Required]
    [ForeignKey("MembershipEntity")]
    public int MemberId { get; set; }

    [Required]
    [ForeignKey("TestEntity")]
    public int TestId { get; set; }

    [Required]
    public int AttemptNumber { get; set; } = 0;

    [Required]
    public int Grade { get; set; } = 0;

    [Required]
    public DateTime DateTimeStarted { get; set; }

    [Required]
    public DateTime DateTimeEnded { get; set; }

    // getters
    public TestEntity TestEntity { get; set; }

    public MembershipEntity Member { get; set; }

    // getters (reverse)
    public ICollection<AnswerEntity> Answers { get; }

}

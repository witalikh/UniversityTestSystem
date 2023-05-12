// <copyright file="ClassroomEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class ClassroomEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [ForeignKey("UserEntity")]
    public string OwnerId { get; set; }

    public UserEntity Owner { get; set; }

    // getters
    public ICollection<MembershipEntity> Members { get; }

    public ICollection<TestEntity> Tests { get; }

    public ICollection<InvitationEntity> Invites { get; set; }
}
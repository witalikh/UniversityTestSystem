// <copyright file="MembershipEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

namespace DataAccessLayer.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MembershipEntity {

    [Required]
    [ForeignKey("UserEntity")]
    public string UserEntityId { get; set; }

    public UserEntity UserEntity { get; set; }

    [Required]
    [ForeignKey("ClassroomEntity")]
    public int ClassroomEntityId { get; set; }

    public ClassroomEntity ClassroomEntity { get; set; }
}
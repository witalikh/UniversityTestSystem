// <copyright file="UserEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models;

public class UserEntity : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }

    [PersonalData]
    public string? LastName { get; set; }

    [PersonalData]
    public string? EducationPlace { get; set; }

    public ICollection<MembershipEntity> Classrooms { get; set; }

    public ICollection<InvitationEntity> Invites { get; set; }
}

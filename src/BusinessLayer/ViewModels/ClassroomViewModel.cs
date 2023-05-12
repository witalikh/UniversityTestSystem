// <copyright file="ClassroomEntity.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels;

public class ClassroomViewModel
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string OwnerId { get; set; } = string.Empty;

    public List<UserViewModel> Members { get; set; } = new List<UserViewModel>();

    // public List<TestViewModel> Tests { get; set; } = new List<TestViewModel>();

    public ClassroomViewModel() {

    }

    public ClassroomViewModel(ClassroomEntity classroom)
    {
        this.Id = classroom.Id;

        this.Name = classroom.Name;

        this.Description = classroom.Description;

        this.OwnerId = classroom.OwnerId;

        if (classroom.Members.Count > 0)
        {
            this.Members = classroom.Members.Select(m => new UserViewModel(m.UserEntity)).ToList();
        }

        // this.Tests = classroom.Tests.Select(t => new TestViewModel(t)).ToList();
    }

    public static ClassroomEntity ToEntity(ClassroomViewModel classroomViewModel)
    {
        if (classroomViewModel.Id != null)
        {
            return new ClassroomEntity
            {
                Id = classroomViewModel.Id.Value,
                Name = classroomViewModel.Name,
                Description = classroomViewModel.Description,
                OwnerId = classroomViewModel.OwnerId,
            };
        }
        return new ClassroomEntity
        {
            Name = classroomViewModel.Name,
            Description = classroomViewModel.Description,
            OwnerId = classroomViewModel.OwnerId,
        };
    }
}
// <copyright file="ApplicationDbContext.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<MembershipEntity> Memberships { get; set; }

    public DbSet<ClassroomEntity> Classrooms { get; set; }

    public DbSet<TestEntity> Tests { get; set; }

    public DbSet<QuestionEntity> Questions { get; set; }

    public DbSet<QuestionChoiceOptionEntity> QuestionChoiceOptions { get; set; }

    public DbSet<AttemptEntity> Attempts { get; set; }

    public DbSet<AnswerEntity> Answers { get; set; }

    public DbSet<AnswerChosenOptionEntity> AnswerChosenOptions { get; set; }

    public DbSet<InvitationEntity> Invitations { get; set; }

    // public DbSet<UserEntity> Users { get; set; }

    public ApplicationDbContext()
    { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MembershipEntity>()
            .HasKey(m =>
            new {
                m.UserEntityId,
                m.ClassroomEntityId,
            });

        builder.Entity<MembershipEntity>()
            .HasOne<UserEntity>(m => m.UserEntity)
            .WithMany(u => u.Classrooms)
            .HasForeignKey(m => m.UserEntityId);

        builder.Entity<MembershipEntity>()
            .HasOne<ClassroomEntity>(m => m.ClassroomEntity)
            .WithMany(m => m.Members)
            .HasForeignKey(m => m.ClassroomEntityId);
    }
}

﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_Angular.Models;

public partial class StudentContext : DbContext
{
    public StudentContext()
    {
    }

    public StudentContext(DbContextOptions<StudentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StudentProfile> StudentProfiles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=RUSSELVIEMWAKIN\\AKEMSSQLSERVER;Database=Student;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.ToTable("studentProfile");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middleName");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SubjectId).HasColumnName("subjectID");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("subjects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StudentId).HasColumnName("studentID");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .HasColumnName("subjectName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolAttendanceManager.Models;

public partial class SchoolAttendanceDbContext : DbContext
{
    public SchoolAttendanceDbContext()
    {
    }

    public SchoolAttendanceDbContext(DbContextOptions<SchoolAttendanceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SUDEEP;Initial Catalog=SchoolAttendanceDB;User ID=sa;Password=Sudeep@123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3213E83FB061C1A0");

            entity.ToTable("Registration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImageUpload)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate).HasColumnName("Registration_Date");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Registrat__RoleI__4CA06362");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__result__3213E83F3E1A0799");

            entity.ToTable("result");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExamResult)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("examResult");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F458D83D6");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__studentD__3213E83F2F8603C3");

            entity.ToTable("studentDetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Class)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("class");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Marks)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Percentage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("percentage");
            entity.Property(e => e.ResultId).HasColumnName("resultId");
            entity.Property(e => e.Section)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Result).WithMany(p => p.StudentDetails)
                .HasForeignKey(d => d.ResultId)
                .HasConstraintName("FK__studentDe__resul__6A30C649");

            entity.HasOne(d => d.Registration).WithMany(p => p.StudentDetails)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__studentDe__Stude__68487DD7");

            entity.HasOne(d => d.Subjects).WithMany(p => p.StudentDetails)
                .HasForeignKey(d => d.SubjectsId)
                .HasConstraintName("FK__studentDe__Subje__693CA210");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3213E83F8BF5D33F");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

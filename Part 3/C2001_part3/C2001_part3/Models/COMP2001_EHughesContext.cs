using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Data.SqlClient;
using C2001_part3.Models;

#nullable disable

namespace C2001_part3.Models
{
    public partial class COMP2001_EHughesContext : DbContext
    {
        public COMP2001_EHughesContext()
        {
        }

        public COMP2001_EHughesContext(DbContextOptions<COMP2001_EHughesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Programme> Programmes { get; set; }
        public virtual DbSet<ProgrammeProject> ProgrammeProjects { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectsAudit> ProjectsAudits { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=socem1.uopnet.plymouth.ac.uk;Database=COMP2001_EHughes;User Id=EHughes;Password=IttI307+");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Programme>(entity =>
            {
                entity.ToTable("programmes", "part3");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("title");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Programmes)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("fk_programme_student_id");
            });

            modelBuilder.Entity<ProgrammeProject>(entity =>
            {
                entity.HasKey(e => new { e.ProgrammeId, e.ProjectId })
                    .HasName("pk_programme_projects");

                entity.ToTable("programme_projects", "part3");

                entity.Property(e => e.ProgrammeId).HasColumnName("programme_id");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.HasOne(d => d.Programme)
                    .WithMany(p => p.ProgrammeProjects)
                    .HasForeignKey(d => d.ProgrammeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_programme_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProgrammeProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_id");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("projects", "part3");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(320)
                    .HasColumnName("description");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("title");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("year");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("fk_project_student_id");
            });

            modelBuilder.Entity<ProjectsAudit>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ProjectId })
                    .HasName("pk_project_audit_id");

                entity.ToTable("projects_audit", "part3");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.AuditActionCode)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("audit_action_code");

                entity.Property(e => e.AuditDateTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("audit_date_time")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(320)
                    .HasColumnName("description");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("title");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("year");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectsAudits)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_audited_project_id");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students", "part3");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //Stored procedure methods:

        public int usp_create_project(Project project, out string _responseMessage)
        {
            SqlParameter response = new SqlParameter("@response_message", SqlDbType.NVarChar, 256);
            response.Direction = ParameterDirection.Output;

            Database.ExecuteSqlRaw("EXEC part3.usp_create_project @student_id, @title, @description, @year, @response_message OUTPUT", //execute usp_create_project, passing in properties from Project object passed into method
                new SqlParameter("@student_id", project.StudentId),
                new SqlParameter("@title", project.Title),
                new SqlParameter("@description", project.Description),
                new SqlParameter("@year", project.Year),
                response);
            _responseMessage = response.Value.ToString();

            if (_responseMessage.Equals("201"))
            {
                using (var _cmd = Database.GetDbConnection().CreateCommand())
                {
                    _cmd.CommandText = "select id from part3.projects where student_id = @student_id and year = @year";
                    _cmd.Parameters.Add(new SqlParameter("@student_id", project.StudentId));
                    _cmd.Parameters.Add(new SqlParameter("@year", project.Year));
                    _cmd.Connection.Open();
                    int _projectId = (int)_cmd.ExecuteScalar();
                    return _projectId;
                }
            }

            return -1;
        }

        public void usp_delete_project(int id)
        {
            Database.ExecuteSqlRaw("EXEC part3.usp_delete_project @id", new SqlParameter("@id", id));
        }

        public void usp_update_project(int project_id, Project project) //the sp was changed from taking in student id to taking in project id as students can upload multiple projects
        {                                                               //using student id could cause multiple, or the wrong project to be updated
            object _title = null;
            object _description = null;
            object _year = null;

            if (String.IsNullOrWhiteSpace(project.Title))
            { //Since title, description and year can all potentially be null 
                _title = DBNull.Value;
            }
            else
            {
                _title = project.Title;
            }

            if (String.IsNullOrWhiteSpace(project.Description))
            {
                _description = DBNull.Value;
            }
            else
            {
                _description = project.Description;
            }

            if (String.IsNullOrWhiteSpace(project.Year)) //if I wasn't constrained by the spec, when year is empty, the program would not execute the stored procedure
            {
                _year = DBNull.Value;
            }
            else
            {
                _year = project.Year;
            }

            Database.ExecuteSqlRaw("EXEC part3.usp_update_project @project_id, @title, @description, @year, @student_id",
                new SqlParameter("@project_id", project_id),
                new SqlParameter("@title", _title),
                new SqlParameter("@description", _description),
                new SqlParameter("@year", _year),
                new SqlParameter("@student_id", project.StudentId));
        }

        public int GetProgrammeProjects(Project project)
        {
            try
            {
                int _numProjects;

                using (var _cmd = Database.GetDbConnection().CreateCommand())
                {
                    _cmd.CommandText = "select id from part3.projects where title = @title and description = @description and year = @year and student_id = @student_id";
                    _cmd.Parameters.Add(new SqlParameter("@title", project.Title));
                    _cmd.Parameters.Add(new SqlParameter("@description", project.Description));
                    _cmd.Parameters.Add(new SqlParameter("@year", project.Year));
                    _cmd.Parameters.Add(new SqlParameter("@student_id", project.StudentId));
                    _cmd.Connection.Open();
                    int _projectId = (int)_cmd.ExecuteScalar();

                    _cmd.Parameters.Clear();
                    _cmd.CommandText = "select programme_id from part3.programme_projects where project_id = @project_id";
                    _cmd.Parameters.Add(new SqlParameter("@project_id", _projectId));
                    int _programmeId = (int)_cmd.ExecuteScalar();

                    _cmd.Parameters.Clear();
                    _cmd.CommandText = "select count(*) from part3.vw_projects_in_programmes where programme_id = @programme_id";
                    _cmd.Parameters.Add(new SqlParameter("@programme_id", _programmeId));
                    _numProjects = (int)_cmd.ExecuteScalar();
                }

                return _numProjects;
            }

            catch (Exception e)
            {
                return -1;
            }
            
        }
    }
}

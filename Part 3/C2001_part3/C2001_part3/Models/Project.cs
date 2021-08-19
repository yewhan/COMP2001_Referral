using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace C2001_part3.Models
{
    public partial class Project
    {
        public Project()
        {
            ProgrammeProjects = new HashSet<ProgrammeProject>();
            ProjectsAudits = new HashSet<ProjectsAudit>();
        }

        public int Id { get; set; }
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
        [JsonPropertyName("Year")]
        public string Year { get; set; }
        [Required]
        [JsonPropertyName("Student_ID")]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<ProgrammeProject> ProgrammeProjects { get; set; }
        public virtual ICollection<ProjectsAudit> ProjectsAudits { get; set; }
    }
}

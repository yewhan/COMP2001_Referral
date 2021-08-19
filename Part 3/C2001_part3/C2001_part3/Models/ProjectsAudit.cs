using System;
using System.Collections.Generic;

#nullable disable

namespace C2001_part3.Models
{
    public partial class ProjectsAudit
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string AuditActionCode { get; set; }
        public DateTime AuditDateTime { get; set; }

        public virtual Project Project { get; set; }
    }
}

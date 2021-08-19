using System;
using System.Collections.Generic;

#nullable disable

namespace C2001_part3.Models
{
    public partial class Programme
    {
        public Programme()
        {
            ProgrammeProjects = new HashSet<ProgrammeProject>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<ProgrammeProject> ProgrammeProjects { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace C2001_part3.Models
{
    public partial class ProgrammeProject
    {
        public int ProgrammeId { get; set; }
        public int ProjectId { get; set; }

        public virtual Programme Programme { get; set; }
        public virtual Project Project { get; set; }
    }
}

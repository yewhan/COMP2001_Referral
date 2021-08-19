using System;
using System.Collections.Generic;

#nullable disable

namespace C2001_part3.Models
{
    public partial class Student
    {
        public Student()
        {
            Programmes = new HashSet<Programme>();
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Programme> Programmes { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}

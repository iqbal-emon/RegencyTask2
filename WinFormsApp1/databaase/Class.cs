using System;
using System.Collections.Generic;

namespace WinFormsApp1.databaase
{
    public partial class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MachineModelWrapper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> MarkId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MachineparkMarkWrapper
    {
        public int Id { get; set; }
        public string MarkName { get; set; }
        public Nullable<bool> IsOwnerMachine { get; set; } = false;
        public Nullable<bool> IsActive { get; set; } = true;
        public Nullable<bool> IsDeleted { get; set; } = false;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class ComboWrapper
    {

        public int Id { get; set; }
        public Nullable<int> ComboTypeId { get; set; }
        public Nullable<int> Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class LocationFilter : Filter
    {

        public int? CustomerId { get; set; }

        public string Name { get; set; }
        public bool IsDelete { get; set; } = false;

        public LocationFilter() : base()
        {
        }
    }
}
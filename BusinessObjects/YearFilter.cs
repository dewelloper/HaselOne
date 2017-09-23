using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class YearFilter : Filter
    {
        public int? Year { get; set; }
        public int? Id { get; set; }
    }
}
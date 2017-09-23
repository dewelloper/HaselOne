using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class SalesmanFilter : Filter
    {
        public int CustomerId { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
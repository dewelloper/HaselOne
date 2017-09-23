using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MachineparkCategoryFilter : Filter
    {
        public int? ParentId { get; set; }
        public int? CustomerId { get; set; }
        public int? SalesmanId { get; set; }
        public bool OnlyRequestVisibleTrue { get; set; } = false;
    }
}
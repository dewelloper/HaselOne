using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MachineparkCategoryWrapper
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string CategoryName { get; set; }
        public int? OrderBy { get; set; }

        public int TreeLevel { get; set; }
        public List<MachineparkCategoryWrapper> Categories { get; set; }

        public MachineparkCategoryWrapper()
        {
        }

        public MachineparkCategoryWrapper(int id, int parentId, string categoryName, int? orderBy)
        {
            Id = id;
            ParentId = parentId;
            CategoryName = categoryName;
            OrderBy = orderBy;
        }
    }
}
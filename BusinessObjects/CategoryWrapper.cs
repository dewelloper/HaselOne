using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class CategoryWrapper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public CategoryWrapper()
        {
        }

        public CategoryWrapper(int id, string title, Nullable<bool> isActive, Nullable<bool> isDeleted)
        {
            Id = id;
            Title = title;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}
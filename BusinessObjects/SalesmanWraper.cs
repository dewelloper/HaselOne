using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class SalesmanWraper
    {
        string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        bool _isAreaDirector = false;

        string _type = "";

        public int Id { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> SalesmanId { get; set; }
        public Nullable<int> SalesmanTypeId { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public bool IsAreaDirector
        {
            get
            {
                return _isAreaDirector;
            }

            set
            {
                _isAreaDirector = value;
            }
        }
    }

    public class FlagDto
    {
        public int CmCustomerSalesmanId { get; set; }
        public bool FlagStatus { get; set; }
    }
}

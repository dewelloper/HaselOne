using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class LocationWrapper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<bool> IsFat { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<int> CreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifierId { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> DeleterId { get; set; }
        public Nullable<System.DateTime> DeleterDate { get; set; }
    }
}
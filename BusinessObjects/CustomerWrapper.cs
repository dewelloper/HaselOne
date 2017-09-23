using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    [Serializable]
    public class CustomerWrapper : BaseWrapper, IBaseWrapper
    {
        public CustomerWrapper()
        {
        }
        private string _strCreatorDate;
        public int Id { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Nullable<bool> IsHasel { get; set; }
        public string NetsisRentliftCode { get; set; }
        public string NetsisHaselCode { get; set; }
        public Nullable<int> SectorId { get; set; }
        public string Web { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> CreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifierId { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public string CreatorName { get; set; }
        public DateTime? CreatorDate { get; set; }

        public string StrCreatorDate
        {
            get
            {
                if (CreateDate != null)
                {
                    return ((DateTime) CreateDate).ToString("dd.MM.yyyy hh:mm:ss.fff");
                }
                return "";
            }
        }

        public string ModifiedName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string StrModifiedDate
        {
            get
            {
                if (ModifiedDate != null)
                {
                    return ((DateTime)ModifiedDate).ToString("dd.MM.yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public CustomerWrapper CmCustomerWrapper { get; set;}

        public string LocationName { get; set; }
        public string AuthenticatorName { get; set; }
        public string SaleEngineeer{ get; set; }

    }
}

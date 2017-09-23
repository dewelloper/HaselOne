using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MachineparkFilter : Filter
    {
        public int CustomerId { get; set; }

        public int? RequestId { get; set; }

        /// <summary>
        /// Makine parkı geçmiş kayıtları getirir.
        /// </summary>
        public bool IsHistoricSearch { get; set; }

        /// <summary>
        /// Elden çıkarılmış kayıtları getirir.
        /// </summary>
        public bool IsReleased { get; set; }

        /// <summary>
        /// Kiralık makine parklarını getirir.
        /// </summary>
        public bool IsRent { get; set; }

        public bool HasRequest { get; set; }

        public DateTime? Begin { get; set; }

        public DateTime? End { get; set; }

        public MachineparkFilter() : base()
        {
            IsHistoricSearch = false;
            IsReleased = false;
            IsRent = false;
            HasRequest = false;
        }
    }
}
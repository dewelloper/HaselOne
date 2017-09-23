using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class SegmentWrapper
    {
        public int Id { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public string Title
        {
            get
            {
                return $"{this.MinValue} - {this.MaxValue}";
            }
        }

        public SegmentWrapper()
        {
        }

        public SegmentWrapper(int id, int minValue, int maxValue)
        {
            Id = id;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
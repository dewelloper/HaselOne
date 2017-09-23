using DAL;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class StatsFilter : Filter
    {
        public CategoryWrapper Category { get; set; }
        public List<AreaWrapper> Areas { get; set; }
        public List<TextValue> Salesmans { get; set; }
        public List<MachineparkCategoryWrapper> MachineparkCategories { get; set; }
        public List<TextValue> Marks { get; set; }
        public List<SegmentWrapper> Segments { get; set; }

        public StatsFilter() : base()
        {
            Areas = new List<AreaWrapper>();
            Salesmans = new List<TextValue>();
            MachineparkCategories = new List<MachineparkCategoryWrapper>();
            Marks = new List<TextValue>();
            Segments = new List<SegmentWrapper>();
        }
    }
}
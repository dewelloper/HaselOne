using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Helper
{
    public static class Helper
    {

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }

        public static Guid GenGuidStaticLife()
        {
            return Guid.NewGuid();
        }

        public static string StaticGuid { get; set; }
    }
    


}

/*
 namespace  Dal
{
    public interface IEntity
    {
        int Id { get; set; }
        int IsDeleted { get; set; }
        int CreateUserId { get; set; }
        DateTime CreateDate { get; set; }
        int? UpdateUserId { get; set; }
        DateTime? UpdateDate { get; set; }
        int? DeleteUserId { get; set; }
        Nullable<DateTime> DeleteDate { get; set; }
        int CustomerId { get; set; }
    }

    public partial class Cm_CustomerRequest:IEntity
    {
        public int Id { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> MarkId { get; set; }
        public Nullable<int> ModelId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int SalesType { get; set; }
        public Nullable<System.DateTime> EstimatedBuyDate { get; set; }
        public Nullable<int> UseDuration { get; set; }
        public Nullable<int> UseDurationUnit { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public int ChannelId { get; set; }
        public Nullable<int> SalesmanId { get; set; }
        public Nullable<bool> ConditionType { get; set; }
        public Nullable<int> MonthlyWorkingHours { get; set; }
        public int ResultType { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public int CustomerId { get; set; }

      
    }

}
     */

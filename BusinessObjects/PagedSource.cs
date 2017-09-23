using System.Collections.Generic;

namespace BusinessObjects
{
    public class PagedSource<TEntity> : IPagedSource<TEntity>
    {
        public int TotalCount { get; set; }
        public ICollection<TEntity> Source { get; set; }
    }

    public interface IPagedSource<TEntity>
    {
        int TotalCount { get; set; }
        ICollection<TEntity> Source { get; set; }
    }
}
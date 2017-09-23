using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaselOne.Services.Interfaces
{
    public interface ICoreService
    {
        IQueryable<Gn_Control> GetControls(int userId, string pageName);

        List<MachineparkCategoryWrapper> GetMachineparkCategories(int? categoryId = null);

        bool SetCategoryByNodeId(int sourceId, int destId, int sourceIndex, int destIndex);

        bool AddNewCategory(string newCategoryName, int parentId);

        bool DeleteCategory(int desCategoryId);
    }
}
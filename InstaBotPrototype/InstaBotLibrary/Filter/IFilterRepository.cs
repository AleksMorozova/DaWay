using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Filter
{
    public interface IFilterRepository
    {
        List<FilterModel> getBoundFilters(int boundId);
        void AddFilter(FilterModel filter);
        void DeleteFilter(FilterModel filter);
        bool findFilter(FilterModel filter);
    }
}

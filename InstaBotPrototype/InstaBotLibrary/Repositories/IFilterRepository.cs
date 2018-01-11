using InstaBotLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Repositories
{
    public interface IFilterRepository
    {
        List<FilterModel> getBoundFilters(int boundId);
        void AddFilter(FilterModel filter);
        void DeleteFilter(FilterModel filter);
    }
}

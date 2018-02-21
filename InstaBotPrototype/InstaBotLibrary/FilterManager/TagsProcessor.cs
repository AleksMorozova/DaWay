using InstaBotLibrary.Filter;
using InstaBotLibrary.Instagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaBotLibrary.FilterManager
{
    public class TagsProcessor
    {
        public List<string> TagIntersection(List<FilterModel> filters, Post post)
        {
            List<string> inputFilters = new List<string>();

            for (int i = 0; i < filters.Count; i++)
            {
                inputFilters.Add(filters[i].Filter);
            }

            return post.tags.Intersect(inputFilters).ToList();
        }
    }
}

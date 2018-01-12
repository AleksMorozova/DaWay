using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotPrototype
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<String> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<String> viewLocations)
        {
            return new[]
            {
            "/UI/Views/{1}/{0}.cshtml",
            "/UI/Views/Shared/{0}.cshtml"
        };
        }
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}

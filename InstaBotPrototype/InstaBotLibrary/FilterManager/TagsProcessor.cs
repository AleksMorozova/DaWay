using InstaBotLibrary.AI;
using InstaBotLibrary.Filter;
using InstaBotLibrary.Instagram;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaBotLibrary.FilterManager
{
    public class TagsProcessor
    {
        private IRecognizer imageRecognizer;
        private IFilterRepository filterRepository;

        public TagsProcessor(IRecognizer _imageRecognizer, IFilterRepository _filterRepository)
        {
            imageRecognizer = _imageRecognizer;
            filterRepository = _filterRepository;
        }

        public async Task<bool> TagIntersectionAsync(Post post, int boundId)
        {
            IEnumerable<string> imageInfo = await imageRecognizer.GetTagsAsync(post.imageUrl);

            List<string> imageInfoList = new List<string>(imageInfo);

            List<string> postTags = post.text.Split('#').ToList();

            List<FilterModel> boundFilters = filterRepository.getBoundFilters(boundId);

            List<string> userFilters = new List<string>();

            foreach (var filter in boundFilters)
            {
                userFilters.Add(filter.Filter);
            }

            if (userFilters.Intersect(postTags).Any() || userFilters.Intersect(imageInfoList).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

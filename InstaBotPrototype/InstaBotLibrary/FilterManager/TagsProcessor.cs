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

        public bool Intersects(IEnumerable<string> tags, IEnumerable<string> filters)
        {
            return tags.Intersect(filters).Any();
        }

        public async Task<bool> TagIntersectionAsync(Post post, int boundId)
        {
            List<FilterModel> boundFilters = filterRepository.getBoundFilters(boundId);

            List<string> filters = boundFilters.ConvertAll(model => model.Filter);

            if (Intersects(post.tags, filters) && Intersects(post.text.Split(' '), filters))
                return true;


            IEnumerable<string> imageInfo = await imageRecognizer.GetTagsAsync(post.imageUrl);

            return Intersects(imageInfo, filters);
        }
    }
}

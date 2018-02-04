using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaBotLibrary.Instagram
{
    public interface IInstagramService
    {
        Task<IEnumerable<string>> GetLatestPosts();
    }
}
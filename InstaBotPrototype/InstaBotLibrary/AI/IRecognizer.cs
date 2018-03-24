using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InstaBotLibrary.AI
{
    public interface IRecognizer
    {
        Task<IEnumerable<string>> GetTagsAsync(string imageUri);
        Task<IEnumerable<string>> GetTagsAsync(Stream imageStream);
    }
}

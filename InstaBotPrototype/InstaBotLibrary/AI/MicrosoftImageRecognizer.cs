using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Vision;

namespace InstaBotLibrary.AI
{
    public class MicrosoftImageRecognizer : IRecognizer
    {
        private MicrosoftVisionOptions options;

        public MicrosoftImageRecognizer(IOptions<MicrosoftVisionOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<IEnumerable<string>> GetTagsAsync(Stream imageStream)
        {
            VisionServiceClient visionServiceClient = new VisionServiceClient(options.apiKey, options.apiRoot);
            var analysisResult = await visionServiceClient.DescribeAsync(imageStream);
            return analysisResult.Description.Tags;
        }

        public async Task<IEnumerable<string>> GetTagsAsync(string imageUri)
        {
            return await GetTagsAsync(imageUri, true);
        }

        private async Task<IEnumerable<string>> GetTagsAsync(string imageUri, bool retry)
        {
            try
            {
                VisionServiceClient visionServiceClient = new VisionServiceClient(options.apiKey, options.apiRoot);
                var analysisResult = await visionServiceClient.DescribeAsync(imageUri);
                return analysisResult.Description.Tags;
            }
            
            catch (ClientException e)
            {
                if (retry)
                {
                    Thread.Sleep(60000);
                    return await GetTagsAsync(imageUri, false);
                }
                else
                {
                    return null;
                }
            }
            
        }

    }
}

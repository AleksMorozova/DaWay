using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            VisionServiceClient visionServiceClient = new VisionServiceClient(options.apiKey, options.apiRoot);
            var analysisResult = await visionServiceClient.DescribeAsync(imageUri);
            return analysisResult.Description.Tags;
        }

    }
}

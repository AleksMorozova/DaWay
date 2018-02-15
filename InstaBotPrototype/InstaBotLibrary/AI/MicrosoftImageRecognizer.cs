using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Vision;

namespace InstaBotLibrary.AI
{
    public class MicrosoftImageRecognizer : IRecognizer
    {
        private MicrosoftVisionOptions options;
        //private string apiKey = "836f89f035b9478d96952e9039695733";
        //private string apiRoot = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";
        public MicrosoftImageRecognizer(IOptions<MicrosoftVisionOptions> options)
        {
            this.options = options.Value;
        }



        public async Task<IEnumerable<string>> GetTagsAsync(string imageUri)
        {
            VisionServiceClient visionServiceClient = new VisionServiceClient(options.apiKey, options.apiRoot);
            var analysisResult = await visionServiceClient.DescribeAsync(imageUri);
            return analysisResult.Description.Tags;
        }

    }
}

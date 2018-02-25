using System;
using System.Collections.Generic;
using System.Text;
using InstaSharp;

namespace InstaBotLibrary.Instagram
{
    static class InstaSharpExtensions
    {
        public static InstagramConfig GetCopy(this InstagramConfig config)
        {
            return new InstagramConfig(
                config.ClientId,
                config.ClientSecret,
                config.RedirectUri,
                config.CallbackUri,
                config.ApiUri,
                config.OAuthUri,
                config.RedirectUri);
        }
    }
}

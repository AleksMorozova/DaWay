using InstaSharp.Endpoints;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstaSharp;
using InstaSharp.Models.Responses;
using Microsoft.Extensions.Configuration;
using InstaSharp.Models;

namespace InstaBotLibrary.Instagram
{
    public class InstagramService
    {
        private InstagramConfig instagramConfig;
        public InstagramService(InstagramConfig config)
        {
            instagramConfig = config;
        }
        public InstagramService(IConfiguration configuration)
        {
            IConfigurationSection instagramAuthSection = configuration.GetSection("InstagramAuth");

            var clientId = instagramAuthSection["client_id"];
            var clientSecret = instagramAuthSection["client_secret"];
            var redirectUri = instagramAuthSection["redirect_uri"];
            var realtimeUri = "";
            instagramConfig = new InstagramConfig(clientId, clientSecret, redirectUri, realtimeUri);
        }


        public string getLoginLink()
        {
            var scopes = new List<OAuth.Scope>();
            scopes.Add(OAuth.Scope.Basic);
            scopes.Add(OAuth.Scope.Public_Content);
            scopes.Add(OAuth.Scope.Follower_List);

            string link = OAuth.AuthLink(instagramConfig.OAuthUri + "authorize", instagramConfig.ClientId, instagramConfig.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);
            return link;
        }

        public async Task<string> GetToken(string code)
        {
            var auth = new OAuth(instagramConfig);

            var oauthResponse = await auth.RequestToken(code);
            return oauthResponse.AccessToken;
        }
        public async Task<MediasResponse> GetMedias(string token)
        {
            var oAuthResponse = await GetOAuthResponse(token);
            var users = new Users(instagramConfig, oAuthResponse);
            MediasResponse feed = await users.RecentSelf();
            return feed;
        }

        private async Task<OAuthResponse> GetOAuthResponse(string token)
        {
            var oAuthResponse = new OAuthResponse() { AccessToken = token, User = new UserInfo() };
            var users = new Users(instagramConfig, oAuthResponse);
            UserResponse userResponse = await users.GetSelf();

            users.OAuthResponse.User = userResponse.Data;
            return users.OAuthResponse;
        }


        public async Task<List<InstaSharp.Models.User>> GetFollowsList(string token)
        {
            var oAuthResponse = await GetOAuthResponse(token);
            Relationships relationships = new Relationships(instagramConfig, oAuthResponse);
            var follows = await relationships.FollowsAll();
            return follows;
        }

        public async Task<List<InstaSharp.Models.Media>> GetFollowsMedia(string token)
        {
            var follows = await GetFollowsList(token);
            return await GetFollowsMedia(token, follows);
        }

        public async Task<List<InstaSharp.Models.Media>> GetFollowsMedia(string token, List<InstaSharp.Models.User> subscriptions)
        {
            OAuthResponse oAuthResponse = await GetOAuthResponse(token);
            var users = new Users(instagramConfig, oAuthResponse);
            List<InstaSharp.Models.Media> medias = new List<InstaSharp.Models.Media>();
            foreach (var user in subscriptions)
            {
                var feed = await users.Recent(user.Id);
                medias.AddRange(feed.Data);
            }
            return medias;
        }
    }
}

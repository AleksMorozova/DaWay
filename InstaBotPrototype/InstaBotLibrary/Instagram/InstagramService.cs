using InstaSharp.Endpoints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstaSharp;
using InstaSharp.Models.Responses;
using InstaSharp.Models;
using Microsoft.Extensions.Options;

namespace InstaBotLibrary.Instagram
{
    public class InstagramService : IInstagramService
    {
        private InstagramConfig instagramConfig;
        private OAuthResponse auth = null;

        public InstagramService(IOptions<InstagramConfig> config)
        {
            instagramConfig = config.Value;
        }

        private void AssertIsAuthenticated()
        {
            if (auth == null)
                throw new InstagramUnauthorizedException();
        }

        public void Auth(OAuthResponse oauth)
        {
            auth = oauth;
        }
        public void Auth(string token, int Id)
        {
            Auth(new OAuthResponse() { AccessToken = token, User = new UserInfo() { Id = Id } });
        }

        private List<OAuth.Scope> getScopes()
        {
            var scopes = new List<OAuth.Scope>();
            scopes.Add(OAuth.Scope.Basic);
            scopes.Add(OAuth.Scope.Public_Content);
            scopes.Add(OAuth.Scope.Follower_List);
            return scopes;
        }
        public string getLoginLink()
        {
            var scopes = getScopes();

            string link = OAuth.AuthLink(instagramConfig.OAuthUri + "authorize", instagramConfig.ClientId, instagramConfig.RedirectUri, scopes, OAuth.ResponseType.Code);
            return link;
        }
        public string getLoginLink(string paramName, string paramValue)
        {
            var scopes = getScopes();
            string link = OAuth.AuthLink(instagramConfig.OAuthUri + "authorize", instagramConfig.ClientId, instagramConfig.RedirectUri + "?" + paramName + "=" + paramValue, scopes, OAuth.ResponseType.Code);
            return link;
        }

        public async Task<string> GetToken(string code)
        {
            var auth = new OAuth(instagramConfig);

            var oauthResponse = await auth.RequestToken(code);
            return oauthResponse.AccessToken;
        }
        public async Task<OAuthResponse> GetResponse(string token, string code)
        {
            var config = instagramConfig.GetCopy();
            config.RedirectUri = instagramConfig.RedirectUri + "?" + "temp_token" + "=" + token;
            var auth = new OAuth(config);
            var oauthResponse = await auth.RequestToken(code);
            return oauthResponse;
        }

        public async Task<MediasResponse> GetMedias()
        {
            AssertIsAuthenticated();
            var users = new Users(instagramConfig, auth);
            MediasResponse feed = await users.RecentSelf();
            return feed;
        }
        public async Task<UserResponse> GerUserInfo()
        {
            AssertIsAuthenticated();
            var users = new Users(instagramConfig, auth);
            return await users.GetSelf();
        }


        public async Task<List<InstaSharp.Models.User>> GetFollowsList()
        {
            AssertIsAuthenticated();
            Relationships relationships = new Relationships(instagramConfig, auth);
            var follows = await relationships.FollowsAll();
            return follows;
        }
        /// <summary>
        /// returns all your follow's media
        /// </summary>
        public async Task<List<InstaSharp.Models.Media>> GetFollowsMedia()
        {
            AssertIsAuthenticated();
            var follows = await GetFollowsList();
            return await GetFollowsMedia(follows);
        }

        public async Task<List<InstaSharp.Models.Media>> GetFollowsMedia(IEnumerable<InstaSharp.Models.User> subscriptions)
        {
            AssertIsAuthenticated();
            var users = new Users(instagramConfig, auth);
            List<InstaSharp.Models.Media> medias = new List<InstaSharp.Models.Media>();
            foreach (var user in subscriptions)
            {
                var feed = await users.Recent(user.Id);
                medias.AddRange(feed.Data);
            }
            return medias;
        }

        public async Task<List<InstaSharp.Models.Media>> GetFollowsMedia(IEnumerable<long> subscriptions)
        {
            AssertIsAuthenticated();
            var users = new Users(instagramConfig, auth);
            List<InstaSharp.Models.Media> medias = new List<InstaSharp.Models.Media>();
            foreach (var userId in subscriptions)
            {
                var feed = await users.Recent(userId);
                medias.AddRange(feed.Data);
            }
            return medias;
        }

        public async Task<IEnumerable<Post>> GetLatestPosts()
        {
            List<InstaSharp.Models.Media> lst = await GetFollowsMedia();
            lst.AddRange((await GetMedias()).Data);
            return Array.ConvertAll(lst.ToArray(), post => new Post(post.Caption.Text, post.Images.StandardResolution.Url, post.Tags));
        }
    }
}

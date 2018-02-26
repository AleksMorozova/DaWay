using InstaSharp.Endpoints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstaSharp;
using InstaSharp.Models.Responses;
using InstaSharp.Models;
using Microsoft.Extensions.Options;
using InstaBotLibrary.Bound;

namespace InstaBotLibrary.Instagram
{
    public class InstagramService : IInstagramService
    {
        private InstagramConfig instagramConfig;
        private IBoundRepository boundRepository;
        private OAuthResponse auth = null;
        private Dictionary<long, string> lastPosts = new Dictionary<long, string>();

        public InstagramService(IOptions<InstagramConfig> config, IBoundRepository boundRepository)
        {
            this.boundRepository = boundRepository;
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
        public void Auth(string token, long Id)
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

        private async Task<List<InstaSharp.Models.Media>> GetMedias()
        {
            AssertIsAuthenticated();
            var users = new Users(instagramConfig, auth);
            MediasResponse feed = await users.RecentSelf();
            List<InstaSharp.Models.Media> medias = new List<InstaSharp.Models.Media>();
            if (feed.Data.Count == 0) return medias;
            if (!lastPosts.ContainsKey(auth.User.Id))
            {
                lastPosts.Add(auth.User.Id, feed.Data[0].Id);
                return feed.Data;
            }
            else
            {
                string lastId = feed.Data[0].Id;
                foreach (var post in feed.Data)
                {
                    if (lastPosts[auth.User.Id] == post.Id)
                        break;
                    medias.Add(post);
                }
                lastPosts[auth.User.Id] = lastId;
            }
            return medias;
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
                if (feed.Data.Count == 0) continue;
                if (!lastPosts.ContainsKey(user.Id))
                {
                    lastPosts.Add(user.Id, feed.Data[0].Id);
                    medias.AddRange(feed.Data);
                }
                else
                {
                    string lastId = feed.Data[0].Id;
                    foreach (var post in feed.Data)
                    {
                        if (lastPosts[user.Id] == post.Id)
                            break;
                        medias.Add(post);
                    }
                    lastPosts[user.Id] = lastId;
                }
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
            List<InstaSharp.Models.Media> lst = new List<InstaSharp.Models.Media>();
            lst.AddRange((await GetMedias()));
            lst.AddRange(await GetFollowsMedia());
            return Array.ConvertAll(lst.ToArray(), post => new Post(post.Caption?.Text, post.Images?.StandardResolution?.Url, post.Tags));
        }
        public async Task<IEnumerable<Post>> GetLatestPosts(int BoundId)
        {
            var bound = boundRepository.GetBoundInfo(BoundId);

            bound.InstagramToken = bound.InstagramToken;
            Auth(bound.InstagramToken, bound.InstagramId.Value);
            return await GetLatestPosts();
        }
    }
}

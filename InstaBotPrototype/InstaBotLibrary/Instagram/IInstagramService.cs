using InstaSharp.Models.Responses;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaBotLibrary.Instagram
{
    public interface IInstagramService
    {
        Task<IEnumerable<Post>> GetLatestPosts();
        void Auth(OAuthResponse oauth);
        void Auth(string token, long Id);
        string getLoginLink();
        string getLoginLink(string paramName, string paramValue);
        Task<OAuthResponse> GetResponse(string token, string code);
        Task<string> GetToken(string code);
        Task<MediasResponse> GetMedias();
        Task<UserResponse> GerUserInfo();
        Task<List<InstaSharp.Models.User>> GetFollowsList();
        Task<List<InstaSharp.Models.Media>> GetFollowsMedia();
        Task<List<InstaSharp.Models.Media>> GetFollowsMedia(IEnumerable<InstaSharp.Models.User> subscriptions);
        Task<List<InstaSharp.Models.Media>> GetFollowsMedia(IEnumerable<long> subscriptions);

    }
}
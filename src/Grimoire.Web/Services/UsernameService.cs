using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Line.Api.Webhook.Source;
using Grimoire.Web.Models;

namespace Grimoire.Web.Services
{
    public class UsernameService
    {
        private readonly Dictionary<string, string> _usernames = new();
        private readonly IBotService _botService;

        public UsernameService(IBotService botService)
        {
            _botService = botService;
        }

        public bool TryGetUsername(string userId, out string username)
            => _usernames.TryGetValue(userId, out username);

        public string SetUsernameCache(string userId, string username)
            => _usernames[username] = username;

        public async Task<string> FetchUsernameAsync(BaseSource source, GrimoireContext context = null)
        {
            var userInfo = _botService.GetUserInfo(source);
            var username = userInfo.displayName;
            _usernames[source.UserId] = username;
            
            if (context == null) return username;
            
            var user = await context.Users.FindAsync(source.UserId);
            
            if (user != null)
                user.LineName = username;
            else
                await context.Users.AddAsync(new User()
                {
                    UserId = source.UserId,
                    LineName = username
                });

            await context.SaveChangesAsync();
            return username;
        }

#pragma warning disable 4014
        public async Task<string> UpdateUsernameCache(BaseSource source, GrimoireContext context)
        {
            var userId = source.UserId;
            
            if (_usernames.TryGetValue(userId, out var username))
            {
                FetchUsernameAsync(source, context);
                return username;
            }
            
            var user = await context.Users.FindAsync(userId);
            username = user?.LineName;
            if (username != null)
            {
                FetchUsernameAsync(source, context);
                return username;
            }

            return await FetchUsernameAsync(source, context);
        }
#pragma warning restore 4014
    }
}
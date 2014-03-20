using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApp.Infrastructure;
using WebApp.Models;
using WebGrease.Css.Extensions;

namespace WebApp.Controllers.Api
{
    public class UsersController : ApiController
    {
        private readonly MegaCache _cache;
        private readonly UsersContext _context;

        public UsersController()
        {
            _cache = new MegaCache();
            _context = new UsersContext();
        }

        public UserProfile GetByUserName(String userName)
        {
            UserProfile result;

            CachePair<DateTime, UserProfile> cacheResult = _cache.Get<CachePair<DateTime, UserProfile>>(userName);
            if (null == cacheResult || cacheResult.First < DateTime.UtcNow.AddMinutes(-5))
            {
                result = LongRunningQuery(userName);
                _cache.Set(userName, new CachePair<DateTime, UserProfile>(DateTime.UtcNow, result));
            }
            else
            {
                result = cacheResult.Second;
            }
            return result;
        }

        public UserProfile GetByUserId(int userId)
        {
            UserProfile result = _cache.Get<UserProfile>(userId.ToString());
            if (null == result)
            {
                result = _context.UserProfiles.SingleOrDefault(u => u.UserId == userId);
                _cache.Set(userId.ToString(), result);
            }
            return result;
        }

        private UserProfile LongRunningQuery(string userName)
        {
            CalculateStat();

            return _context.UserProfiles.SingleOrDefault(u => u.UserName == userName);
        }

        private static void CalculateStat()
        {
            Task first = Task.Run(() => ProcessData());
            Task second = Task.Run(() => ProcessData());

            ProcessData();

            Task.WaitAll(first, second);
        }

        private static void ProcessData()
        {
            Enumerable.Range(0, 10000).ForEach(i => Trace.Write(Math.Pow(Math.E, Math.PI) + i));
        }
    }
}
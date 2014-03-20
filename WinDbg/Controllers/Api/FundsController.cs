using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApp.Infrastructure;

namespace WebApp.Controllers.Api
{
    public class FundsController : ApiController
    {
        private static readonly Random Rand;

        private readonly Lazy<ILogger> _logger;

        static FundsController()
        {
            Rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public FundsController(Lazy<ILogger> logger)
        {
            _logger = logger;
        }

        public async Task<int> GetFunds(int accountId)
        {
            _logger.Value.Log("Funds request for " + accountId);

            await Task.Delay(TimeSpan.FromMilliseconds(300));

            return Rand.Next(0, 10000);
        }
    }
}
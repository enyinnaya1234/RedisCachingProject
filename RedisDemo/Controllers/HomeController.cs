using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisDemo.Models;
using System.Diagnostics;

namespace RedisDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public HomeController(IDistributedCache distributedCache)
        {
                _distributedCache = distributedCache;
        }
        public async Task<ActionResult> SaveRedisCache()
        {
            var dashboardData = new DashboardData
            {
                TotalCustomerCount = 2100670,
                TotalRevenue = 19980,
                TopSellingCountryName = "Nigeria",
                TopSellingProductName = "Ijebu Garri"
            };
            var tomorrow = DateTime.Now.Date.AddDays(1);
            var totalSeconds = tomorrow.Subtract(DateTime.Now).TotalSeconds;
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(totalSeconds);
            distributedCacheEntryOptions.SlidingExpiration = null;

            var jsonData =  JsonConvert.SerializeObject(dashboardData);
            await _distributedCache.SetStringAsync("DashboardData", jsonData, distributedCacheEntryOptions);
             return View();
        }
    }
}

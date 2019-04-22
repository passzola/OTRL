using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CUSTOR.OTRLS.Core
{
    public class ZoneRepository  
    {
        OTRLSDbContext context;
        private readonly IDistributedCache distributedCache;
        private readonly Settings settings;
        public ZoneRepository(OTRLSDbContext _context, IDistributedCache _distributedCache, IConfiguration _configuration)
        {
            settings = new Settings(_configuration);
            context = _context;
            distributedCache = _distributedCache;
        }
        public async Task<IEnumerable<StaticData2>> GetAllZones(string lang)
        {
            IEnumerable<StaticData2> zones = null;
            string cacheKey = "Zones";
            var cachedZones = await distributedCache.GetStringAsync(cacheKey);

            if (cachedZones != null)
            {
                zones = JsonConvert.DeserializeObject<IEnumerable<StaticData2>>(cachedZones);
            }
            else
            {
                zones = await context.Zone
                .Where(z => z.IsActive == true && z.IsDeleted == false)
                .Select(z => new StaticData2
                {
                    Id = z.ZoneId,
                    ParentId = z.RegionId,
                    Description = (lang == "et") ? z.Description : z.DescriptionEnglish
                })
                .ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(settings.ExpirationPeriod));
                await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(zones), cacheOptions);
            }

            return zones;
        }
 
    }
}
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
    public class RegionRepository
    {
        OTRLSDbContext context;
        private readonly IDistributedCache distributedCache;
        private readonly Settings settings;
        public RegionRepository(OTRLSDbContext _context, IDistributedCache _distributedCache, IConfiguration _configuration)
        {
            settings = new Settings(_configuration);
            context = _context;
            distributedCache = _distributedCache;
        }
        

        public async Task<IEnumerable<StaticData2>> GetRegions(string lang)
        {
            IEnumerable<StaticData2> regions = null;
            string cacheKey = "Regions";
            var cachedRegions = await distributedCache.GetStringAsync(cacheKey);

            if (cachedRegions != null)
            {
                regions = JsonConvert.DeserializeObject<IEnumerable<StaticData2>>(cachedRegions);
            }
            else
            {
                regions = await context.Region
                .Where(r => r.IsActive == true && r.IsDeleted == false)
                .Select(r => new StaticData2
                {
                    Id = r.RegionId,
                    Description = (lang == "et") ? r.Description : r.DescriptionEnglish
                })
                .ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(settings.ExpirationPeriod));
                await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(regions), cacheOptions);
            }

            return regions;
           
        }


    }
}
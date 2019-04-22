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
    public class KebeleRepository
    {
        OTRLSDbContext context;
        private readonly IDistributedCache distributedCache;
        private readonly Settings settings;
        public KebeleRepository(OTRLSDbContext _context, IDistributedCache _distributedCache, IConfiguration _configuration)
        {
            settings = new Settings(_configuration);
            context = _context;
            distributedCache = _distributedCache;
        }
        public async Task<IEnumerable<StaticData2>> GetKebelesByWoredaId(string lang, string parentId)
        {
            IEnumerable<StaticData2> kebeles = null;
            string cacheKey = "Kebeles: " + parentId;
            var cachedKebeles = await distributedCache.GetStringAsync(cacheKey);

            if (cachedKebeles != null)
            {
                kebeles = JsonConvert.DeserializeObject<IEnumerable<StaticData2>>(cachedKebeles);
            }
            else
            {
                kebeles = await context.Kebele
                    .Where(k => k.WoredaId == parentId && k.IsActive == true && k.IsDeleted == false)
                    .Select(k => new StaticData2
                    {
                        Id = k.WoredaId,
                        Description = (lang == "et") ? k.Description : k.DescriptionEnglish
                    })
                    .ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(settings.ExpirationPeriod));
                await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(kebeles), cacheOptions);
            }

            return kebeles;
        }

    }
}
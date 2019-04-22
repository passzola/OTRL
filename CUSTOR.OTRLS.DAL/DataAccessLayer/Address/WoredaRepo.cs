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
    public class WoredaRepository
    {
        OTRLSDbContext context;
        private readonly IDistributedCache distributedCache;
        private readonly Settings settings;
        public WoredaRepository(OTRLSDbContext _context, IDistributedCache _distributedCache, IConfiguration _configuration)
        {
            settings = new Settings(_configuration);
            context = _context;
            distributedCache = _distributedCache;
        }
        public async Task<IEnumerable<StaticData2>> GetAllWoredas(string lang)
        {
            IEnumerable<StaticData2> woredas = null;
            string cacheKey = "Woredas";
            var cachedWoredas = await distributedCache.GetStringAsync(cacheKey);

            if (cachedWoredas != null)
            {
                woredas = JsonConvert.DeserializeObject<IEnumerable<StaticData2>>(cachedWoredas);
            }
            else
            {
                woredas = await context.Woreda
                .Where(w => w.IsActive == true && w.IsDeleted == false)
                .Select(w => new StaticData2
                {
                    Id = w.WoredaId,
                    ParentId = w.ZoneId,
                    Description = (lang == "et") ? w.Description : w.DescriptionEnglish
                })
                .ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(settings.ExpirationPeriod));
                await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(woredas), cacheOptions);
            }

            return woredas;
        }
      
    }
}
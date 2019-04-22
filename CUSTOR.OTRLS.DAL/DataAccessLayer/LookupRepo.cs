using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace CUSTOR.OTRLS.Core
{
    public class LookupRepository
    {
        OTRLSDbContext context;
        private readonly IDistributedCache distributedCache;
        private readonly Settings settings;
        public LookupRepository(OTRLSDbContext _context, IDistributedCache _distributedCache, IConfiguration _configuration)
        {
            settings = new Settings(_configuration);
            context = _context;
            distributedCache = _distributedCache;
        }
        public async Task<List<Lookup>> GetAllLookup()
        {

            IQueryable<Lookup> lookups = context.Lookup
                                            .OrderBy(l => l.English);

            return await lookups.ToListAsync();

        }

       

        public async Task<List<Lookup>> GetLookupByParent(int id, int page = 0, int pageSize = 15)
        {
            IQueryable<Lookup> lookups = context.Lookup
                .Where(sp => sp.LookUpTypeId == id)
                .OrderBy(sp => sp.English);
            if (page > 0)
            {
                lookups = lookups
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }

            return await lookups.ToListAsync();
        }

       
        public async Task<List<LookupDTO>> GetAllLookupByLang(string lang)
        {

            return await context.Lookup
                .Select(l => new LookupDTO

                {
                    LookupId = l.LookupId,
                    LookUpTypeId = l.LookUpTypeId,
                    Description = (lang == "et") ? l.Amharic : l.English
                })
                .ToListAsync();

        }
        public async Task<IEnumerable<StaticData>> GetLookups(string lang, int parentId)
        {
            IEnumerable<StaticData> lookups = null;
            string cacheKey = "Lookups: " + parentId;
            var cachedLookups = await distributedCache.GetStringAsync(cacheKey);

            if (cachedLookups != null)
            {
                lookups = JsonConvert.DeserializeObject<IEnumerable<StaticData>>(cachedLookups);
            }
            else
            {
                lookups = await context.Lookup
                .Where(l => l.LookUpTypeId == parentId && l.IsActive == true && l.IsDeleted == false)
                .Select(l => new StaticData
                {
                    Id = l.LookupId,
                    Description = (lang == "et") ? l.Amharic : l.English
                })
                .ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(settings.ExpirationPeriod));
                await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(lookups), cacheOptions);
            }

            return lookups;
        }
        public  async Task<Lookup> GetLookup(object LookupId)
        {
            Lookup lookups = null;

            int id = (int)LookupId;
            lookups = await context.Lookup
                .Where(look => look.LookupId == id).FirstOrDefaultAsync();

            return lookups;
        }


        public async Task<ICollection<Lookup>> GetRecordByParent(object LookupId)
        {
            ICollection<Lookup> lookups = null;

            int id = (int)LookupId;
            lookups = await context.Lookup
                .Where(look => look.LookUpTypeId == id).ToListAsync();

            return lookups;
        }

        public async Task<bool> DeleteLookup(int id)
        {
            var Lookup = await context.Lookup
                .FirstOrDefaultAsync(lookup => lookup.LookupId == id);
            if (Lookup == null)
            {
                throw new Exception("Lookup does not exist");
             }

            context.Lookup.Remove(Lookup);
            return true;
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CUSTOR.OTRLS.Core
{
    public class Settings
    {
        public int ExpirationPeriod = 9000;        
        public Settings(IConfiguration configuration)
        {
            int expirationPeriod;
            if (Int32.TryParse(configuration["Caching:ExpirationPeriod"], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out expirationPeriod))
            {
                ExpirationPeriod = expirationPeriod;
            }
        }
    }
}

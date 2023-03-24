using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Domain.Settings
{
    public class JWTSettings
    {
        #region Properties
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int DurationTime { get; set; }

        #endregion

        #region Constructor
        public JWTSettings(string secret, string validIssuer, string validAudience, int durationTime)
        {
            Secret = secret;
            ValidIssuer = validIssuer;
            ValidAudience = validAudience;
            DurationTime = durationTime;
        }
        #endregion
    }
}

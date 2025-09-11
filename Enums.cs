using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAChatFlow.Shared.Common
{
    public static  class Enums
    {
        public enum ConsentStatus
        {
            PENDING,
            VALIDATED,
            REJECTED,
            COOLDOWN,
            REVOKED
        }
    }
}

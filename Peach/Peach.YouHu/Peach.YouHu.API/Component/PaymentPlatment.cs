using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Component
{
    using System.Threading.Tasks;

    public class PaymentPlatment
    {
        private static PaymentPlatment _default = new PaymentPlatment();

        public static PaymentPlatment Default
        {
            get
            {
                return _default;
            }
        }

        public async Task<bool> Pay(decimal cost)
        {
            return await Task.Run<bool>(() => true);
        }
    }
}
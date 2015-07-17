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

        public async Task<bool> Pay(decimal cost,string remitter)
        {
            return await Task.Run<bool>(() => true);
        }

        public async Task<bool> WithDraw(decimal amount, string reciever)
        {
            return await Task.Run(() => true);
        }
    }
}
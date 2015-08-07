using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Component
{
    public class ConsignCode
    {
        private static ConsignCode _keygen = new ConsignCode();

        public static ConsignCode Keygen
        {
            get
            {
                return _keygen;
            }
        }

        public int Generate()
        {
            int code;
            if(!int.TryParse(DateTime.Now.ToString("ffffff"),out code)) code=123456;
            return code;
        }
    }
}
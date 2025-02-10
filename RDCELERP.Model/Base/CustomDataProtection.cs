using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDCELERP.Model.Base
{
    public class CustomDataProtection
    {
        private readonly IDataProtector protector;

        public CustomDataProtection(IDataProtectionProvider dataProtectionProvider, UniqueCode uniqueCode)
        {
            protector = dataProtectionProvider.CreateProtector(uniqueCode.BankIdRouteValue);
        }

        public string Decode(object data)
        {
            string? maindata = data.ToString();
            return protector.Unprotect(maindata);
        }

        public string Encode(object data)
        {
            string? maindata = data.ToString();
            return protector.Protect(maindata);
        }




    }

    public class UniqueCode
    {
        public readonly string BankIdRouteValue = "Id";
    }
}


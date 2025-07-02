using RDCELERP.Model.RazorPayX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
public interface IRazorpayXService
    {
        public  Task<RazorpayContactInfo> GetContactAsync(string regdNo, string token);

        public Task<RazorpayFundAccountInfo> GetFundAccountAsync(string contactId, string token);

        public  Task<RazorpayContactInfo> CreateContactAsync(object contactPayload);

        public Task<RazorpayFundAccountInfo> CreateFundAccountAsync(object fundAccountPayload);

        public Task<RazorpayPayoutResponse> MakePayoutAsync(object payoutPayload, string basicAuthToken);
        public string GenerateToken(string keyId, string keySecret);
    }
}

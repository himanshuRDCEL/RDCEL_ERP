using NPOI.POIFS.Crypt.Standard;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.Model.CashfreeModel;
using RDCELERP.Model.Paymant;

namespace RDCELERP.BAL.Interface
{
   public interface ICashfreePayoutCall
   {
        public string Rest_ResponseCashfreeAuthCall(string url, Method methodType);
        public string Rest_ResponseCashfreeServiceCall(string url, Method methodType, string AuthToken, object content = null);
        public CashfreeAuth CashFreeAuthCall();
        public AddBeneficiaryResponse AddBenefiaciary(AddBeneficiary addBaneficiary,string AuthToken);

        public GetBeneficiary GetBeneficiary(string AuthToken,string BeneficiaryId);

        public RemoveBeneficiaryResponse RemoveBeneficiary(RemoveBeneficiary removeBeneficiary,string AuthToken);
        public string Rest_ResponseCashfreeRemoveBeneficiaryCall(string url, Method methodType, string AuthToken, RemoveBeneficiary removeBeneficiary);

        public TransactionResponseCashfree Transaction(ProcessTransactionCashfree transaction,string AuthToken);
        public string Rest_ResponseCashfreeGetBeneficiaryCall(string url, Method methodType, string AuthToken);

        public TransactionCashFree CashFreeTransaction(string regdno, string AuthToken);
        public string Rest_ResponseCashFreeTransaction(string url, Method methodType, string Authtoken);
        public TransactionCashFree GetPaymentTransferStatus(string UtcReferenceID, string AuthToken);
        public string Rest_ResponseCashFreeGetPaymentStatus(string url, Method methodType, string AuthToken);
        public GetWalletBalanceResponse GetWalletBalance(string AuthToken);
        public string Rest_ResponseCashFreeGetWalletBalance(string url, Method methodType, string AuthToken);
    }
}

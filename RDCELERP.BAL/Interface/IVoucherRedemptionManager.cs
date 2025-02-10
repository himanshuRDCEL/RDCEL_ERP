using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RDCELERP.Model.DealerDashBoard;

using RDCELERP.Model;
using RDCELERP.Model.ExchangeOrder;
using RDCELERP.Model.MobileApplicationModel;
using RDCELERP.Model.MobileApplicationModel.Questioners;
using RDCELERP.Model.QCComment;
using Newtonsoft.Json;
using RestSharp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static RDCELERP.Model.Whatsapp.WhatsappSelfqcViewModel;
using RDCELERP.BAL.MasterManager;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Enums;
using RDCELERP.DAL.Entities;
using RDCELERP.Model.VoucherRedemption;

namespace RDCELERP.BAL.Interface
{
    public interface IVoucherRedemptionManager
    {

        public bool sendSelfQCUrl(string regdNo, string mobnumber, int? loginid);
        public VoucherDataContract VerifyVoucherCode(VoucherDataContract voucherDataContract);

        public ExchangeOrderDataContract GetExchangeOrderDCByVoucherCode(string vcode, string custphone);

        public int AddVouchertoDB(ExchangeOrderDataContract voucherData);


    }
}

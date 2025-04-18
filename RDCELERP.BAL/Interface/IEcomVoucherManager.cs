using RDCELERP.Model.EcomVoucher;
using RDCELERP.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Interface
{
    public interface IEcomVoucherManager

    {/// <summary>
     /// Method to manage (Add/Edit) voucher 
     /// </summary>
     /// <param name="EcomVM">EcomVM</param>
     /// <param name="EcomVMId">EcomVMId</param>
     /// <returns>int</returns>
       public bool ManageEcomVoucher(EcomVoucherViewModel EcomVM, int userId, int? companyId);


        /// <summary>
        /// Method to get the EcomVM by id 
        /// </summary>
        /// <param name="id">EcomVMId</param>
        /// <returns>EcomVoucherViewModel</returns>
      public  EcomVoucherViewModel GetEcomVoucherById(int id);

    }
}

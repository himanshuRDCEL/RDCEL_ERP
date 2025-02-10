using RDCELERP.BAL.Interface;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Users;
using System;
using RDCELERP.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RDCELERP.Common.Enums;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Helper;
using RDCELERP.Model.Master;
using RDCELERP.Model.OrderTrans;
using RDCELERP.Model.EVC_Allocated;
using RDCELERP.DAL.Repository;

namespace RDCELERP.BAL.MasterManager
{
    public class OrderTransactionManager : IOrderTransactionManager
    {
        #region  Variable Declaration
        IOrderTransRepository _OrderTransRepository;
        private readonly IMapper _mapper;
        ILogging _logging;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        IOrderQCRepository _orderQCRepository;
        ICommonManager _commonManager;

        #endregion

        public OrderTransactionManager(IOrderTransRepository OrderTransRepository, IMapper mapper, ILogging logging, IOrderQCRepository orderQCRepository, ICommonManager commonManager)
        {
            _OrderTransRepository = OrderTransRepository;
            _mapper = mapper;
            _logging = logging;
            _orderQCRepository = orderQCRepository;
            _commonManager = commonManager;
        }

        /// <summary>
        /// Method to manage (Add/Edit) OrderTransaction 
        /// </summary>
        /// <param name="OrderTransactionVM">OrderTransactionVM</param>
        /// <param name="userId">userId</param>
        public int ManageOrderTransaction(OrderTransactionViewModel OrderTransactionVM, int userId = 3)
        {
            TblOrderTran TblOrderTran = new TblOrderTran();
            int orderid = 0;
            try
            {
                if (OrderTransactionVM != null)
                {
                    TblOrderTran = _mapper.Map<OrderTransactionViewModel, TblOrderTran>(OrderTransactionVM);


                    if (TblOrderTran.OrderTransId > 0)
                    {
                        //code to update
                        TblOrderTran.ModifiedBy = 3;
                        TblOrderTran.ModifiedDate = _currentDatetime;
                        _OrderTransRepository.Update(TblOrderTran);
                    }
                    else
                    {
                        //code to create
                        TblOrderTran.IsActive = true;
                        TblOrderTran.CreatedDate = _currentDatetime;
                        TblOrderTran.ModifiedDate = _currentDatetime;
                        TblOrderTran.CreatedBy = 3;
                        TblOrderTran.StatusId = Convert.ToInt32(OrderStatusEnum.OrderCreatedbySponsor);
                        _OrderTransRepository.Create(TblOrderTran);
                    }
                    _OrderTransRepository.SaveChanges();
                    orderid = TblOrderTran.OrderTransId;


                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("OrderTransactionManager", "ManageOrderTransaction", ex);
            }
            return orderid;
        }

        #region get order details by regdno
        /// <summary>
        /// get order details by regdno
        /// </summary>
        /// <param name="regdno"></param>
        /// <returns>tblOrderTran</returns>
        public TblOrderTran GetOrderDetailsByRegdNo(string regdno)
        {
            TblOrderTran tblOrderTran = null;
            try
            {
                if (!string.IsNullOrEmpty(regdno))
                {
                    tblOrderTran = _OrderTransRepository.GetRegdno(regdno);
                    if (tblOrderTran != null)
                    {
                        return tblOrderTran;
                    }
                    else
                    {
                        return tblOrderTran = new TblOrderTran();
                    }
                }
                else
                {
                    return tblOrderTran = new TblOrderTran();
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("OrderTransactionManager", "GetOrderDetailsByRegdNo", ex);
            }
            return tblOrderTran = new TblOrderTran();
        }
        #endregion

        #region Get Order details by OrderTransId
        /// <summary>
        /// Get Order details by OrderTransId
        /// </summary>
        /// <param name="orderTransId"></param>
        /// <returns> orderTransViewModel</returns>
        public Allocate_EVCFromViewModel GetOrderDetailsByOrderTransId(int? orderTransId)
        {
            OrderTransViewModel? orderTransViewModel = new OrderTransViewModel();
            TblOrderTran? tblOrderTrans = null;
            TblOrderQc? tblOrderQc = null;
            Allocate_EVCFromViewModel allocate = new Allocate_EVCFromViewModel();
            try
            {
                if (orderTransId > 0)
                {
                    tblOrderTrans = _OrderTransRepository.GetOrderDetailsByOrderTransId(Convert.ToInt32(orderTransId));
                    if (tblOrderTrans != null)
                    {
                        allocate.ActualBasePrice = tblOrderTrans.FinalPriceAfterQc != null ? tblOrderTrans.FinalPriceAfterQc : 0;
                        
                        allocate.RegdNo = tblOrderTrans.RegdNo;
                        allocate.orderTransId = tblOrderTrans.OrderTransId;
                        allocate.Ordertype = (int)tblOrderTrans.OrderType;
                        allocate.SweetenerAmt = tblOrderTrans.Sweetner ?? 0;
                        if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.ABB))
                        {
                            if (tblOrderTrans.Abbredemption?.Abbregistration != null)
                            {
                                allocate.CustPin = tblOrderTrans.Abbredemption.Abbregistration.CustPinCode;
                                allocate.FirstName = tblOrderTrans.Abbredemption.Abbregistration.CustFirstName;
                                allocate.CustCity = tblOrderTrans.Abbredemption.Abbregistration.CustCity;
                                allocate.Custstate = tblOrderTrans.Abbredemption.Abbregistration.CustState;
                                allocate.OldProdType = tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategoryTypeNavigation?.Description;
                            }
                            if (tblOrderTrans.Abbredemption?.Abbregistration?.NewProductCategory != null)
                            {
                                allocate.ExchProdGroup = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory.Description;
                                allocate.ProductCatId = tblOrderTrans.Abbredemption.Abbregistration.NewProductCategory.Id;
                            }
                        }
                        else if (tblOrderTrans.OrderType == Convert.ToInt32(LoVEnum.Exchange))
                        {
                            if (tblOrderTrans.Exchange?.CustomerDetails != null)
                            {
                                allocate.CustPin = tblOrderTrans.Exchange.CustomerDetails.ZipCode;
                                allocate.FirstName = tblOrderTrans.Exchange.CustomerDetails.FirstName;
                                allocate.CustCity = tblOrderTrans.Exchange.CustomerDetails.City;
                                allocate.Custstate = tblOrderTrans.Exchange.CustomerDetails.State;
                                allocate.Bonus = tblOrderTrans.Exchange.Bonus;

                            }
                            if (tblOrderTrans.Exchange?.ProductType?.ProductCat != null)
                            {

                                allocate.ExchProdGroup = tblOrderTrans.Exchange.ProductType.ProductCat.Description;
                                allocate.ProductCatId = tblOrderTrans.Exchange.ProductType.ProductCat.Id;
                            }
                            if (tblOrderTrans.Exchange.ProductType != null)
                            {
                                allocate.OldProdType = !string.IsNullOrEmpty(tblOrderTrans.Exchange.ProductType.Description) ? tblOrderTrans.Exchange.ProductType.Description : string.Empty;
                            }
                        }
                        tblOrderQc = _orderQCRepository.GetQcorderBytransId(Convert.ToInt32(orderTransId));
                        if (tblOrderQc != null)
                        {
                            allocate.ActualProdQltyAtQc = tblOrderQc.QualityAfterQc;
                        }
                        else
                        {
                            allocate.ActualProdQltyAtQc = string.Empty;
                        }
                        if (allocate.ActualBasePrice > 0 && allocate.ActualBasePrice != null)
                        {
                            bool IsEVCAmtWithSweetener = true;
                            var result = _commonManager.CalculateEVCPriceNew(tblOrderTrans.OrderTransId, false);
                            var result1 = _commonManager.CalculateEVCPriceNew(tblOrderTrans.OrderTransId, IsEVCAmtWithSweetener);
                            allocate.ExpectedPrice = result > 0 ? result : 0;
                            allocate.ExpectedPriceWithSweetener = result1 > 0 ? result1 : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("OrderTransactionManager", "GetOrderDetailsByOrderTransId", ex);
            }
            return allocate;
        }

        #endregion

        #region assign order for qc
        public string AssignOrderForQC(int adminUserId, int selectUser, string orderIds)
        {
            string message = "Success";

            try
            {
                if (string.IsNullOrEmpty(orderIds))
                {
                    return "OrderIds parameter is null or empty.";
                }

                // Split the string by comma to create a string array
                string[] valueArray = orderIds.Split(',');

                // Convert the string array to a list if needed
                var valueList = new List<string>(valueArray);

                // Now valueList will contain individual values as elements
                if (valueList.Count > 0)
                {
                    foreach (string value in valueList)
                    {
                        int ordertransId;
                        if (int.TryParse(value, out ordertransId) && ordertransId > 0)
                        {
                            TblOrderTran tblOrderTran = _OrderTransRepository.GetSingle(x => x.IsActive==true && x.OrderTransId == ordertransId);
                            if (tblOrderTran != null && selectUser > 0 && adminUserId > 0)
                            {
                                tblOrderTran.AssignTo = selectUser;
                                tblOrderTran.AssignBy = adminUserId;
                                tblOrderTran.ModifiedBy = adminUserId;
                                tblOrderTran.ModifiedDate = DateTime.Now.AddTicks(-(DateTime.Now.Ticks % TimeSpan.TicksPerSecond)); // Remove milliseconds
                                _OrderTransRepository.Update(tblOrderTran);
                            }
                        }
                    }
                    int result = _OrderTransRepository.SaveChanges(); // Move outside the loop to improve efficiency
                    if (result > 0)
                    {
                        return "Successfully Assigned";
                    }
                }
                else
                {
                    return "OrderIds list is empty.";
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("OrderTransactionManager", "ManageOrderTransaction", ex);
                message = "An error occurred while processing orders.";
            }
            return message;
        }
            //public string AssignOrderForQC(int adminUserId,int selectUser,string orderIds)
            //{
            //    TblOrderTran TblOrderTran = new TblOrderTran();
            //    int orderId = 0;
            //    string message ="Success";
            //    try
            //    {

            //        //convert ids into list

            //        // Split the string by comma to create a string array
            //        string[] valueArray = orderIds.Split(',');

            //        // Convert the string array to a list if needed
            //        var valueList = new System.Collections.Generic.List<string>(valueArray);

            //        // Now valueList will contain individual values as elements
            //        if (valueList!=null && valueList.Count > 0)
            //        {
            //            foreach (string value in valueList)
            //            {
            //                int ordertransId = Convert.ToInt32(value);
            //                if (ordertransId > 0)
            //                {
            //                    TblOrderTran tblOrderTran = _OrderTransRepository.GetSingle(x => x.IsActive == true && x.OrderTransId == ordertransId);
            //                    if (tblOrderTran != null && selectUser>0 && adminUserId>0)
            //                    {
            //                        tblOrderTran.AssignTo = selectUser;
            //                        tblOrderTran.AssignBy = adminUserId;
            //                        tblOrderTran.ModifiedBy = adminUserId;
            //                        tblOrderTran.ModifiedDate = DateTime.Now.TrimMilliseconds();
            //                        _OrderTransRepository.Update(tblOrderTran);
            //                        _OrderTransRepository.SaveChanges();
            //                    }
            //                }
            //            }

            //        }
            //        else
            //        {

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        _logging.WriteErrorToDB("OrderTransactionManager", "ManageOrderTransaction", ex);
            //    }
            //    return message;
            //}
            #endregion
        }
}

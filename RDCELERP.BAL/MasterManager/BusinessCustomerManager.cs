using RDCELERP.BAL.Interface;
using RDCELERP.DAL.IRepository;
using RDCELERP.Model.Zoho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using RDCELERP.DAL.Entities;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Base;
using AutoMapper;
using Microsoft.Extensions.Options;
using static RDCELERP.Common.Helper.MessageHelper;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Common.Constant;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Users;

namespace RDCELERP.BAL.MasterManager
{
    public class BusinessCustomerManager :IBusinessCustomerManager
    {
        #region  Variable Declaration
        IBussinessCustomerRepository _businessCustomerRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        private CustomDataProtection _protector;
        IErrorLogManager _errorLogManager;
        IMailManager _mailManager;
        IMapper _mapper;
        ILogging _logging;
        IOptions<ApplicationSettings> _config;
        #endregion
        #region Constructor
        public BusinessCustomerManager(IBussinessCustomerRepository businessCustomerRepository,CustomDataProtection protector,
        IErrorLogManager errorLogManager,IMailManager mailManager,
        IMapper mapper,ILogging logging,IOptions<ApplicationSettings> config) {
            _businessCustomerRepository = businessCustomerRepository;
            _errorLogManager = errorLogManager;
            _mailManager = mailManager;
            _mapper = mapper;
            _logging = logging;
            _config = config;
            _protector = protector;

        }
        #endregion




        /// <summary>
        /// Method to get the customer object by login detail
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>LoginViewModel</returns>
        public LoginViewModel GetCustomerByLogin(string username, string password)
        {
            BusinessCustomerViewModel UserVM = null;
            TblBusinessCustomer TblBusinessCustomer = null;
            LoginViewModel loginVM = null;
            try
            {
               
                TblBusinessCustomer = _businessCustomerRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(username.ToLower())) && (x.Password != null && x.Password.Equals(password)));
                if (TblBusinessCustomer != null)
                {

                    UserVM = _mapper.Map<TblBusinessCustomer, BusinessCustomerViewModel>(TblBusinessCustomer);
                    if (UserVM != null)
                    {
                        TblBusinessCustomer.LastLogin = _currentDatetime;

                        _businessCustomerRepository.Update(TblBusinessCustomer);
                        _businessCustomerRepository.SaveChanges();

                        loginVM = new LoginViewModel();
                        loginVM.BusinessCustomerViewModel = UserVM;

                    }
                }
            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessCustomerManager", "GetCustomerByLogin", ex);
            }
            return loginVM;
        }
        /// <summary>
        /// method to manager zoho customer
        /// </summary>
        /// <param name="bussinessCustomerVM"></param>
        /// <param name="bussinessCustomerId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int ManageBusinessCustomer(BusinessCustomerViewModel bussinessCustomerVM)
        {
            TblBusinessCustomer TblBussinessCustomer = new TblBusinessCustomer();
            try
            {
                if (bussinessCustomerVM != null)
                {
                    TblBussinessCustomer = _mapper.Map<BusinessCustomerViewModel, TblBusinessCustomer>(bussinessCustomerVM);
                    TblBussinessCustomer.Password = TblBussinessCustomer.Password?.Trim();
                    TblBussinessCustomer.Email = TblBussinessCustomer.Email?.Trim();
                    TblBussinessCustomer.LastName = TblBussinessCustomer.LastName?.Trim();
                    TblBussinessCustomer.FirstName = TblBussinessCustomer.FirstName?.Trim();
                    TblBussinessCustomer.FullName = TblBussinessCustomer.FullName?.Trim();
                    if (TblBussinessCustomer.BusinessCustomerId > 0)
                    {
                        //Code to update the object
                        TblBussinessCustomer.PhoneNo = SecurityHelper.EncryptString(TblBussinessCustomer.PhoneNo, _config.Value.SecurityKey);
                        TblBussinessCustomer.Email = SecurityHelper.EncryptString(TblBussinessCustomer.Email, _config.Value.SecurityKey);
                        TblBussinessCustomer.Password = SecurityHelper.EncryptString(TblBussinessCustomer.Password, _config.Value.SecurityKey);

                        // TblBussinessCustomer.ModifiedBy = bussinessCustomerId;
                        TblBussinessCustomer.ModifiedDate = _currentDatetime;
                        _businessCustomerRepository.Update(TblBussinessCustomer);
                        _businessCustomerRepository.SaveChanges();

                    }
                    else
                    {
                        bussinessCustomerVM.UnEncPassword = StringHelper.RandomStrByLength(6);
                        
                        var Check = _businessCustomerRepository.GetSingle(x => x.Email == TblBussinessCustomer.Email);
                        if (Check == null)
                        {
                            //Code to Insert the object
                           
                            TblBussinessCustomer.Email = SecurityHelper.EncryptString(TblBussinessCustomer.Email,_config.Value.SecurityKey);
                            TblBussinessCustomer.Password = SecurityHelper.EncryptString(bussinessCustomerVM.UnEncPassword, _config.Value.SecurityKey);
                            TblBussinessCustomer.IsActive = true;
                            TblBussinessCustomer.CreatedDate = _currentDatetime;
                           _businessCustomerRepository.Create(TblBussinessCustomer);
                            _businessCustomerRepository.SaveChanges();

                            //send email
                            if (bussinessCustomerVM.BusinessCustomerId == 0)
                            {

                                TblBussinessCustomer.Password = !string.IsNullOrEmpty(bussinessCustomerVM.UnEncPassword) ? SecurityHelper.DecryptString(TblBussinessCustomer.Password, _config.Value.SecurityKey) : string.Empty;
                                TblBussinessCustomer.Email = SecurityHelper.DecryptString(TblBussinessCustomer.Email, _config.Value.SecurityKey);
                                BussinessCustomerNotification(TblBussinessCustomer, "Welcome to Rocking Deals", EmailTemplateConstant.NewbussinessCustomerAdded);

                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                _logging.WriteErrorToDB("BusinessCustomerManager", "ManageBusinessCustomer", ex);
            }

            return TblBussinessCustomer.BusinessCustomerId;
        }
        public async void BussinessCustomerNotification(TblBusinessCustomer customerObj, string subject, string tempateName)


        {
            //bool flag = false;
            string toEmails = string.Empty;
            string content = string.Empty;
            try
            {
                if (customerObj != null)
                {

                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HTMLTemplate", tempateName);
                    content = File.ReadAllText(file);
                    content = content.Replace("[ToName]", customerObj.FirstName);

                    content = content.Replace("[Email]", customerObj.Email);
                    content = content.Replace("[Password]", customerObj.Password);
                    content = content.Replace("[BaseURL]", _config.Value.B2BBaseURL);
                    content = content.Replace("[SupportEmail]", "rdcel");
                    toEmails = string.IsNullOrEmpty(toEmails) ? customerObj.Email : toEmails + ";" + customerObj.Email;
                }
                await _mailManager.SingleSendEmailAsync(toEmails, content, subject);

            }
            catch (Exception ex)
            {
                _errorLogManager.WriteErrorToLogAsync("BusinessCustomerManager", "UserNotification", ex);
            }
        }

    }
}

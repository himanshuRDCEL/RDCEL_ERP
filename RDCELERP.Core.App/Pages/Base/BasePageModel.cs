using RDCELERP.Core.App.Helper;
using RDCELERP.Model.Base;
using RDCELERP.Model.Role;
using RDCELERP.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using RDCELERP.Common.Enums;

namespace RDCELERP.Core.App.Pages.Base
{
    public class BasePageModel : PageModel
    {

        public LoginViewModel? _loginSession;
        public readonly IOptions<ApplicationSettings> _baseConfig;
        AccessRuleViewModel accessRule = new AccessRuleViewModel();
        private IOptions<ApplicationSettings> config;

        public BasePageModel(IOptions<ApplicationSettings> baseConfig)
        {
            _baseConfig = baseConfig;
        }
        private void InitRepository()
        {
            _loginSession = SessionHelper.GetObjectFromJson<LoginViewModel>(HttpContext.Session, "LoginUser");

        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            //ApplicationSettings objAapplicationSettings = new ApplicationSettings();
            string? URLPrefixforProd = _baseConfig?.Value?.URLPrefixforProd; 
            ViewData["URLPrefixforProd"] = URLPrefixforProd;

            string? InvoiceImageURL = _baseConfig?.Value?.InvoiceImageURL;
            ViewData["InvoiceImageURL"] = InvoiceImageURL;

            string? ExchangeImagesURL = _baseConfig?.Value?.ExchangeImagesURL;
            ViewData["ExchangeImagesURL"] = ExchangeImagesURL;

            InitRepository();
            if (_loginSession != null)
            {
                ViewData["LoginUser"] = _loginSession;
                ViewData["SelectedCompanyId"] = _loginSession.RoleViewModel?.CompanyId != null ? _loginSession.RoleViewModel.CompanyId : 0;
                ViewData["RoleId"] = _loginSession.RoleViewModel?.RoleId;
                ViewData["CompanyId"] = _loginSession.RoleViewModel?.CompanyId;
                ViewData["LoggedInUserId"] = _loginSession.UserViewModel?.UserId;
                ViewData["BusinessUnitVM"] = _loginSession.RoleViewModel?.BusinessUnitViewModel;
                //ViewData["BusinessUnitId"] = _loginSession.BusinessUnitViewModel.BusinessUnitId;
                ViewData["RoleAccessVMList"] = _loginSession.RoleViewModel?.RoleAccessViewModelList;
                /*ViewData["UrlNew"] =*/
                var FullUrl = context.ActionDescriptor.ViewEnginePath;
                ViewData["UrlNew"] = FullUrl;
                if (_loginSession.RoleViewModel?.RoleName == "Super Admin")
                {
                    accessRule = new AccessRuleViewModel();
                    accessRule.CanAdd = true;
                    accessRule.CanDelete = true;
                    accessRule.CanView = true;
                    ViewData["AccessRule"] = accessRule;

                }
                else
                {
                    if (context.ActionDescriptor.ViewEnginePath != "/Dashboard" && context.ActionDescriptor.ViewEnginePath != "/Company/SelectCompany")
                    {
                        ////url for server
                        //var url = "/Dosolar"+context.ActionDescriptor.ViewEnginePath;
                        ////url for test server
                        //  var url = "/TestDosolar" + context.ActionDescriptor.ViewEnginePath;
                        //url for local
                        var url = URLPrefixforProd + context.ActionDescriptor.ViewEnginePath;
                        var urlforcheck = context.ActionDescriptor.ViewEnginePath; // temp solution, to be corrected later
                        accessRule = new AccessRuleViewModel();
                        RoleAccessViewModel? roleAccessVM = _loginSession?.RoleViewModel?.RoleAccessViewModelList?.FirstOrDefault(x => x.ActionUrl != null &&  x.ActionUrl.ToLower() == urlforcheck.ToLower());
                        if (roleAccessVM == null)
                        {
                            accessRule.CanView =true;

                            //Write the code to say not authorized 
                            //ViewData["UserAuth"] = "You Are Not Permitted This Page."; // URL: " + url;
                        }
                        else
                        {
                            
                            accessRule.CanAdd = roleAccessVM.CanAdd;
                            accessRule.CanDelete = roleAccessVM.CanDelete;
                            accessRule.CanView = roleAccessVM.CanView;

                            //if(!accessRule.CanAdd || !accessRule.CanView)
                            //{
                            //    accessRule.Message("Warning!", "warning", "You Are Not Permitted This Action");

                            //}
                            ViewData["AccessRule"] = accessRule;
                        }
                        if (!accessRule.CanView)
                        {
                            var urlWithPrefix = URLPrefixforProd + "/PermissionDenied";
                            context.Result = new RedirectResult(urlWithPrefix);
                        }
                    }
                }
            }
            else
            {
                HttpContext.Session.Clear();
                context.Result = new RedirectResult(_baseConfig?.Value?.BaseURL);
                //  context.Result = new RedirectResult("/");
            }


        }

        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewData["MessageType"] = messageType;
            ModelState.AddModelError(string.Empty, message);
        }
    }
}

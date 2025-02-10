using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;
using RDCELERP.Core.App.Pages.Base;
using Microsoft.Extensions.Options;
using RDCELERP.Model.Base;
using RDCELERP.Common.Helper;
using RDCELERP.Model.Users;

namespace RDCELERP.Core.App.Pages.Users
{
    public class DetailsModel : BasePageModel
    {
        private readonly RDCELERP.DAL.Entities.Digi2l_DevContext _context;
        public readonly IOptions<ApplicationSettings> _config;
        private CustomDataProtection _protector;


        public DetailsModel(RDCELERP.DAL.Entities.Digi2l_DevContext context, IOptions<ApplicationSettings> config, CustomDataProtection protector)
        : base(config)
        {
            _context = context;
            _config = config;
            _protector = protector;
        }

        public TblUser TblUser { get; set; }
        public IList<TblUserRole> TblUserRole { get; set; }
       
        public IActionResult OnGetAsync(string id )
        {
            
            if (_loginSession == null)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }
                id = _protector.Decode(id);

                TblUser = _context.TblUsers
                    .Include(t => t.CreatedByNavigation)
                    .Include(t => t.ModifiedByNavigation).FirstOrDefault(m => m.IsActive == true && m.UserId == Convert.ToInt32(id));
                if(TblUser != null)
                {

                    if (TblUser.Phone != null)
                    {
                        TblUser.Phone = SecurityHelper.DecryptString(TblUser.Phone, _config.Value.SecurityKey);
                    }
                    if (TblUser.Email != null)
                    {
                        TblUser.Email = SecurityHelper.DecryptString(TblUser.Email, _config.Value.SecurityKey);
                    }
                    if (TblUser.Password != null)
                    {
                        TblUser.Password = SecurityHelper.DecryptString(TblUser.Password, _config.Value.SecurityKey);
                    }



                }

                if (TblUser == null)
                {
                    return NotFound();
                }
                else
                {

                    TblUserRole = _context.TblUserRoles
                        .Where(m => m.UserId == (Convert.ToInt32(id)))
                         .Include(t => t.Role)
                         .Include(t => t.Company).Where(x => x.IsActive == true).ToList();


                    

                   
                   

                }

                return Page();
            }
        }
    }
}

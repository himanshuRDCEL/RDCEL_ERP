using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.BAL.Helper
{
    public class ValidateABBCertificatesOnServerPath
    {
        public static void GetUrlsforCertificates(ref string actionUrl, string BaseUrl, string RegdNo, List<string> AllPresentedfilePaths)
        {

            if (!string.IsNullOrEmpty(actionUrl) && !string.IsNullOrEmpty(BaseUrl) && !string.IsNullOrEmpty(RegdNo))
            {
                bool result = false;
                string fileName = $"{RegdNo}_Certificate.pdf";

                if (AllPresentedfilePaths != null && AllPresentedfilePaths.Count() > 0)
                {
                    if (AllPresentedfilePaths.Contains(fileName))
                    {
                        result = true;
                    }
                }

                string returnUrl = "CompanyDashBoard/ABBDashBoard";
                string message = "Your Certificate is not Generated yet";
                if (result)
                {
                    actionUrl = actionUrl + "<a href ='" + BaseUrl + "/DBFiles/ABB/ABBApprovalCertificate/" + RegdNo + "_Certificate.pdf" + "'target=\"_blank\"><button class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='ViewCertificate'><i class='fa-solid fa-file-pdf'></i></button></a>&nbsp;";
                    actionUrl = actionUrl + "<a href ='" + BaseUrl + "/DBFiles/ABB/ABBApprovalCertificate/" + RegdNo + "_Certificate.pdf" + "' download><button class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Download'><i class='fa-solid fa-download'></i></button></a>&nbsp;";
                }
                else
                {
                    actionUrl = actionUrl + "<a href ='" + BaseUrl + "/ABB/CertificateNotFoundDetails/?message=" + message + "&ReturnURL=" + returnUrl + "'target=\"_blank\"><button class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='ViewCertificate'><i class='fa-solid fa-file-pdf'></i></button></a>&nbsp;";
                    actionUrl = actionUrl + "<a href ='" + BaseUrl + "/ABB/CertificateNotFoundDetails/?message=" + message + "&ReturnURL=" + returnUrl + "'target=\"_blank\"><button class='btn btn-sm btn-primary' data-bs-toggle='tooltip' data-bs-placement='top' title='Download'><i class='fa-solid fa-download'></i></button></a>&nbsp;";
                }
            }
        }
    }
}
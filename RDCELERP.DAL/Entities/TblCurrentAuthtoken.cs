using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblCurrentAuthtoken
    {
        public int CurrentAuthtokenId { get; set; }
        public string? CurrentAuthTokenName { get; set; }
        public string? CurrentAuthToken { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

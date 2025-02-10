using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class TblUnInstallationPriceMaster
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public string? Size { get; set; }
        public string? Type { get; set; }
        public decimal? UninstallationPrice { get; set; }
        public string? ProductType { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace RDCELERP.DAL.Entities
{
    public partial class ViewAbbCount
    {
        public int? CountBu { get; set; }
        public int BusinessUnitId { get; set; }
        public string? Name { get; set; }
    }
}

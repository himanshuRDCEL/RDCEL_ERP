using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Model.Dashboards
{
    public class QCDashboardViewModel
    {
        public int CountOrderForQC { get; set; }
        public int CountSelfQCOrders { get; set; }
        public int CountAllResheduledQC { get; set; }
        public int CountResheduledQCAging { get; set; }
        public int CountResheduledQCCount1 { get; set; }
        public int CountResheduledQCCount2 { get; set; }
        public int CountResheduledQCCount3 { get; set; }
        public int CountPriceQuotedQC { get; set; }
        public int CountCancelledOrders { get; set; }
        public int CountReopenedQC { get; set; }
        public string? OrderForQCUrl { get; set; }
        public string? SelfQCOrdersUrl { get; set; }
        public string? AllResheduledQCUrl { get; set; }
        public string? ResheduledQCAgingUrl { get; set; }
        public string? ResheduledQC1Url { get; set; }
        public string? ResheduledQC2Url { get; set; }
        public string? ResheduledQC3Url { get; set; }
        public string? PriceQuotedQCUrl { get; set; }
        public string? CancelledOrdersUrl { get; set; }
        public string? ReopenedQCUrl { get; set; }
    }
}

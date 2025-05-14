using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloud_app_dev_exam_project.Models
{
    public class PurchaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<PurchaseErrorDetail> FailedItems { get; set; } = new();
    }

    public class PurchaseErrorDetail
    {
        public int ItemId { get; set; }
        public string Reason { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling.Models
{
    public class LeaveRequest
    {
        public string WorkerNumber { get; set; }
        public string PositionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string LegalEntityId { get; set; }
        public string LeaveTypeId { get; set; }
        public int TransactionType { get; set; }
        public string TransactionNumber { get; set; }
        public string LeavePlanId { get; set; }
        public string ExecutionID { get; set; }
        public string Partition { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class Employment
    {
        public string WorkerNumber { get; set; }
        public string PositionId { get; set; }
        public string LegalEntityId { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime EmploymentEndDate { get; set; }
        public string ExecutionIdEmp { get; set; }
        public string PartitionEmp { get; set; }
        public byte WorkerStarterFlag { get; set; }
    }
}

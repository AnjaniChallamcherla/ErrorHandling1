using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class Termination
    {
        public string WorkerNumber { get; set; }
        public string PositionId { get; set; }
        public string LegalEntityId { get; set; }
        public DateTime TransitionDate { get; set; }
        public DateTime LastDateWorked { get; set; }
        public string ExecutionIdEmpDetail { get; set; }
        public string PartitionEmpDetail { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime EmploymentEndDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool EmploymentEnded { get; set; }
    }
}

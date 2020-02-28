using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class EmployeeDetail
    {
        public string WorkerNumber { get; set; }
        public string PositionId { get; set; }
        public string LegalEntityId { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime EmploymentEndDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ProbationPeriod { get; set; }
        public string ExecutionId { get; set; }
        public string Partition { get; set; }
    }
}

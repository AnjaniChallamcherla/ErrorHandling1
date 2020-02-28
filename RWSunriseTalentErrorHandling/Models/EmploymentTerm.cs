using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{

    public class EmploymentTerm
    {
        public string WorkerNumber { get; set; }
        public string PositionId { get; set; }
        public string LegalEntityId { get; set; }
        public string AgreementTermId { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime EmploymentEndDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ExecutionEmpTerm { get; set; }
        public string PartitionEmpTerm { get; set; }
    }
}

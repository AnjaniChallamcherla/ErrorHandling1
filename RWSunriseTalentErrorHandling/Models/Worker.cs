using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{

    public class Worker
    {
        public string WorkerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string EmailAddressOnSite { get; set; }
        public string PreferredFirstName { get; set; }
        public string PreferredLastName { get; set; }
        public string ProfessinalTitle { get; set; }
        public string ProfessionalSuffix { get; set; }
        public string Salutation { get; set; }
        public string Floor { get; set; }
        public string ExecutionIdWorker { get; set; }
        public string PartitionWorker { get; set; }
        public DateTime SeniorityDate { get; set; }
        public Employment WorkerEmployment { get; set; }
        public Position WorkerPosition { get; set; }
        public PositionAssignment WorkerPositionAssignment { get; set; }
        public PositionHierarchy WorkerPositionHierarchy { get; set; }
        public EmploymentTerm WorkerEmploymentTerm { get; set; }
        public PositionFinancialDimension WorkerPositionFinancialDimension { get; set; }
        public Termination WorkerEmploymentDetail { get; set; }
        public CreateUpdateDateTime WorkerDateTimeCreatedUpdated { get; set; }
        public EmployeeDetail EmployeeDetail { get; set; }
    }
}

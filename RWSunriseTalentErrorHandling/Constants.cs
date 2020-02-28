using TalentErrorHandling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    internal class Constants
    {
        internal const string TableSchema = "dbo";
        internal const string WorkerErrorHandlingTable = "WORKER_ERROR_HANDLING";
        internal const string EmploymentStagingTable = "HcmEmploymentStaging";
        internal const string WorkerStagingHistoryTable = "HcmWorkerStaging_H";
        internal const string EmploymentDetailStagingHistoryTable = "HcmEmploymentDetailStaging_H";
        internal const string EmploymentEmployeeStagingHistoryTable = "HcmEmploymentEmployeeStaging_H";
        internal const string EmploymentTermStagingHistoryTable = "HcmEmploymentTermStaging_H";
        internal const string PositionDefaultDimensionStagingHistoryTable = "HcmPositionDefaultDimensionStaging_H";
        internal const string PositionHierarchyStagingHistoryTable = "HcmPositionHierarchyStaging_H";
        internal const string PositionV2StagingHistoryTable = "HcmPositionV2Staging_H";
        internal const string PositionWorkerAssignmentStagingHistoryTable = "HcmPositionWorkerAssignmentStaging_H";
        internal static readonly string LegalEntities = Environment.GetEnvironmentVariable("Legal_Entities", EnvironmentVariableTarget.Process);

        internal static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        internal List<string> WorkerMandatory()
        {
            //Mandatory Fields
            List<string> workerMandatoryFields = new List<string> {
            "WorkerNumber",
            "FirstName",
            "LastName",
            "SeniorityDate",
            "WorkerPositionFinancialDimension.JobTitle",
            "WorkerPositionFinancialDimension.JobLevel",
            "WorkerPositionFinancialDimension.FeeEarner",
            "WorkerPositionFinancialDimension.ConsultantType",
            "WorkerEmploymentTerm.AgreementTermId",
            "WorkerDateTimeCreatedUpdated.CreatedDateTime",
            "WorkerPositionAssignment.PositionId",
            "WorkerPositionAssignment.ValidFrom",
            "WorkerPosition.DepartmentNumber",
            "WorkerPosition.LocationName",
            "WorkerPosition.DepartmentName"
        };
            return workerMandatoryFields;
        }
        internal List<string> PostitionFDMandatory()
        {
            List<string> postionFDmandatoryFields = new List<string> {
            "WorkerNumber",
            "JobTitle",
            "JobLevel",
            "FeeEarner",
            "ConsultantType"
        };
            return postionFDmandatoryFields;
        }
        internal List<string> EmploymentTermMandatory()
        {
            List<string> employmentTermmandatoryFields = new List<string> {
            "WorkerNumber",
            "AgreementTermId"
        };
            return employmentTermmandatoryFields;
        }
        internal List<string> PositionAssignmentMandatory()
        {
            List<string> positionAssignmentmandatoryFields = new List<string> {
            "WorkerNumber",
            "PositionId",
            "ValidFrom"
        };
            return positionAssignmentmandatoryFields;
        }
        internal List<string> PositionMandatory()
        {
            List<string> positionmandatoryFields = new List<string> {
            "WorkerNumber",
            "DepartmentNumber",
            "LocationName",
            "DepartmentName"
        };
            return positionmandatoryFields;
        }
        internal List<LenghtRestrictions> WorkerLenghtFields()
        {
            //Length Restriction Fields
            List<LenghtRestrictions> lenghtRestrictions = new List<LenghtRestrictions> {
               new LenghtRestrictions{FieldName="ProfessionalSuffix",Length=50},
               new LenghtRestrictions{FieldName="ProfessinalTitle",Length=50},
               new LenghtRestrictions{FieldName="EmailAddress",Length=50},
               new LenghtRestrictions{FieldName="FirstName",Length=30},
               new LenghtRestrictions{FieldName="LastName",Length=30},
               new LenghtRestrictions{FieldName="PreferredFirstName",Length=30},
               new LenghtRestrictions{FieldName="PreferredLastName",Length=30},
               new LenghtRestrictions{FieldName="WorkerPosition.TitleId",Length=50}
            };
            return lenghtRestrictions;
        }
        internal List<LenghtRestrictions> PositionLenghtFields()
        {
            //Length Restriction Fields
            List<LenghtRestrictions> lenghtRestrictions = new List<LenghtRestrictions> {
               new LenghtRestrictions{FieldName="TitleId",Length=50}
            };
            return lenghtRestrictions;
        }
    }
}
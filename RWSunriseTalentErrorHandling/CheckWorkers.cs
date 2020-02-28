using TalentErrorHandling.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentErrorHandling
{
    internal class CheckWorkers
    {
        public async Task<string> ReadPropertiesRecursiveAsync(object worker, string workerType)
        {
            Constants cons = new Constants();
            UpdateData updateData = new UpdateData();

            List<string> mandatoryFields = null;
            switch (workerType)
            {
                case "Workers":
                    mandatoryFields = cons.WorkerMandatory();
                    break;
                case "PositionFinancialDimensions":
                    mandatoryFields = cons.PostitionFDMandatory();
                    break;
                case "PositionAssignments":
                    mandatoryFields = cons.PositionAssignmentMandatory();
                    break;
                case "Positions":
                    mandatoryFields = cons.PositionMandatory();
                    break;
                case "EmploymentTerms":
                    mandatoryFields = cons.EmploymentTermMandatory();
                    break;
            }
            List<LenghtRestrictions> lenghtRestrictions = null;
            switch (workerType)
            {
                case "Workers":
                    lenghtRestrictions = cons.WorkerLenghtFields();
                    break;
                case "PositionFinancialDimensions":
                    lenghtRestrictions = cons.PositionLenghtFields();
                    break;
            }


            string mandatoryCheck = string.Empty;
            string lengthRestrictionsCheck = string.Empty;


            //Check for Mandatory Fields
            if (mandatoryFields != null)
            {
                foreach (string mandatoryField in mandatoryFields)
                {
                    Tuple<string, string> result1 = worker.GetPropValue(mandatoryField);

                    if (result1.Item1 == "String")
                    {
                        if (result1.Item2 == null || result1.Item2 == "")
                        {
                            mandatoryCheck = mandatoryCheck + mandatoryField + ", ";
                        }
                    }
                    else if (result1.Item1 == "DateTime")
                    {
                        if (result1.Item2.ToString() == "01/01/0001 00:00:00" ||
                            result1.Item2.ToString() == "01/01/1900 00:00:00" ||
                            result1.Item2.ToString() == "1/1/0001 12:00:00 AM" ||
                            result1.Item2.ToString() == "1/1/1900 12:00:00 AM"
                            )
                        {
                            mandatoryCheck = mandatoryCheck + mandatoryField + ", ";
                        }
                    }
                    else if (result1.Item1 == string.Empty)
                    {
                        mandatoryCheck = mandatoryCheck + mandatoryField + ", ";
                    }
                }
            }
            //Check for LengthRestrictions
            if (lenghtRestrictions != null)
            {
                foreach (LenghtRestrictions lengthRestriction in lenghtRestrictions)
                {
                    Tuple<string, string> result1 = worker.GetPropValue(lengthRestriction.FieldName);

                    if (result1.Item2 != null)
                    {
                        if (result1.Item2.Length > lengthRestriction.Length)
                        {
                            lengthRestrictionsCheck = lengthRestrictionsCheck + lengthRestriction.FieldName
                                                          + $" shouldn't be more than {lengthRestriction.Length} characters. ";
                        }
                    }

                }
            }

            if (!string.IsNullOrEmpty(mandatoryCheck))
            {
                mandatoryCheck = "The following fields are mandatory: " + mandatoryCheck + ".";
            }
            string res = "valid data";

            res = await updateData.UpdateSQLTable(worker, mandatoryCheck, lengthRestrictionsCheck, workerType);

            return res;
        }
    }
}

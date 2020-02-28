using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace TalentErrorHandling
{
    internal class UpdateData
    {

        public async Task<string> UpdateSQLTable(object worker, string mandatoryFieldsCheck, string lengthRestrictionFieldsCheck, string workerType)
        {
            string result = "valid data";
            string workerNumber = worker.GetPropValue("WorkerNumber").Item2;
            string legalEntityID = string.Empty;
            if (workerType == "Workers")
            {
                legalEntityID = worker.GetPropValue("WorkerEmploymentDetail.LegalEntityId").Item2;
            }
            else
            {
                legalEntityID = worker.GetPropValue("LegalEntityId").Item2;
            }
            //check if Worker is present in worker_error_handling table
            using (SqlConnection conn = new SqlConnection(SQLConn.ConnStr()))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"select * from [{Constants.TableSchema}].[{Constants.WorkerErrorHandlingTable}] " +
                    $"where WORKERNUMBER = @workernumber " +
                    $"and MAIL_SENT = @mail_sent");

                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@workernumber", workerNumber);
                    command.Parameters.AddWithValue("@mail_sent", 'N');

                    await conn.OpenAsync();

                    using (SqlDataReader dr = await command.ExecuteReaderAsync())
                    {
                        //Get LegalEntityID
                        if (legalEntityID == string.Empty)
                        {
                            sb.Clear();
                            sb.Append($"select LEGALENTITYID from [{Constants.TableSchema}].[{Constants.EmploymentStagingTable}] " +
                                $" Where PERSONNELNUMBER = @workerNumber" +
                                $" and LEGALENTITYID in ({Constants.LegalEntities})");


                            using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                            {
                                cmd.Parameters.AddWithValue("@workerNumber", workerNumber);
                                using (SqlDataReader datarow = await cmd.ExecuteReaderAsync())
                                {
                                    if (datarow.HasRows)
                                    {
                                        while (await datarow.ReadAsync())
                                        {
                                            legalEntityID = datarow["LEGALENTITYID"].ToString();
                                        }
                                    }
                                }
                            }

                        }
                        //if worker present, update data
                        if (dr.HasRows)
                        {
                            //if mandatoryField or lengthRestrictions are not empty, add worker to worker_error_handling table
                            if (!string.IsNullOrEmpty(mandatoryFieldsCheck) || !string.IsNullOrEmpty(lengthRestrictionFieldsCheck))
                            {
                                sb.Clear();
                                sb.Append($"UPDATE [{Constants.TableSchema}].[{Constants.WorkerErrorHandlingTable}]" +
                                    $" SET MANDATORYFIELDSCHECK = @mandatoryFieldsCheck," +
                                    $" LENGTHRESTRICTIONSCHECK = @lengthRestrictionFieldsCheck," +
                                    $" LEGALENTITYID = @legalEntityId" +
                                    $" where WORKERNUMBER = @workerNumber");

                                using (SqlCommand commandUpdate = new SqlCommand(sb.ToString(), conn))
                                {
                                    commandUpdate.Parameters.AddWithValue("@workerNumber", workerNumber);
                                    commandUpdate.Parameters.AddWithValue("@legalEntityId", legalEntityID);
                                    commandUpdate.Parameters.AddWithValue("@mandatoryFieldsCheck", mandatoryFieldsCheck);
                                    commandUpdate.Parameters.AddWithValue("@lengthRestrictionFieldsCheck", lengthRestrictionFieldsCheck);
                                    await commandUpdate.ExecuteNonQueryAsync();
                                }

                                result = $"{workerNumber}:Invalid Data.";
                            }
                            else
                            {
                                sb.Clear();
                                sb.Append($"DELETE from [{Constants.TableSchema}].[{Constants.WorkerErrorHandlingTable}]" +
                                   $" where WORKERNUMBER = @workerNumber " +
                                   $"and MAIL_SENT = @mail_sent");

                                using (SqlCommand commandUpdate = new SqlCommand(sb.ToString(), conn))
                                {
                                    commandUpdate.Parameters.AddWithValue("@workerNumber", workerNumber);
                                    commandUpdate.Parameters.AddWithValue("@mail_sent", 'N');
                                    await commandUpdate.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(mandatoryFieldsCheck) || !string.IsNullOrEmpty(lengthRestrictionFieldsCheck))
                            {
                                //create worker
                                sb.Clear();
                                sb.Append($"insert into [{Constants.TableSchema}].[{Constants.WorkerErrorHandlingTable}] " +
                                    $"([WORKERNUMBER],[LEGALENTITYID],[MANDATORYFIELDSCHECK],[LENGTHRESTRICTIONSCHECK],[MAIL_SENT])" +
                                    $" values(@workerNumber,@legalEntityId, @mandatoryFieldsCheck, @lengthRestrictionFieldsCheck, @mail_sent )");

                                using (SqlCommand commandUpd = new SqlCommand(sb.ToString(), conn))
                                {
                                    commandUpd.Parameters.AddWithValue("@workerNumber", workerNumber);
                                    commandUpd.Parameters.AddWithValue("@legalEntityId", legalEntityID);
                                    commandUpd.Parameters.AddWithValue("@mandatoryFieldsCheck", mandatoryFieldsCheck);
                                    commandUpd.Parameters.AddWithValue("@lengthRestrictionFieldsCheck", lengthRestrictionFieldsCheck);
                                    commandUpd.Parameters.AddWithValue("@mail_sent", 'N');

                                    await commandUpd.ExecuteNonQueryAsync();
                                    result = $"{workerNumber}:Invalid Data.";
                                }
                            }
                        }
                    }
                    //reset To_Be_Processed flag in few tables, so that sync tool wont pick this user in next run
                    await ResetToBeProcessedFlag(worker, workerNumber, workerType);
                }
                return result;
            }
        }

        public async Task ResetToBeProcessedFlag(object worker, string workerNumber, string workerType)
        {

            string ExecutionID = worker.GetPropValue("ExecutionIdWorker").Item2;
            string positionID = worker.GetPropValue("WorkerPosition.PositionId").Item2;
            //Get ExecutionIdWorker
            string workerExecutionID, EmploymentDetailExecutionID, EmploymentEmployeeExecutionID, EmploymentTermExecutionID, PositionDefaultDimensionExecutionID, PositionHierarchyExecutionID, PositionV2ExecutionID, PositionWorkerAssignmentExecutionID = null;
            switch (workerType)
            {
                case "Workers":
                    workerExecutionID = worker.GetPropValue("ExecutionIdWorker").Item2;
                    EmploymentDetailExecutionID = worker.GetPropValue("WorkerEmploymentDetail.ExecutionIdEmpDetail").Item2;
                    EmploymentEmployeeExecutionID = worker.GetPropValue("EmployeeDetail.ExecutionId").Item2;
                    EmploymentTermExecutionID = worker.GetPropValue("WorkerEmploymentTerm.ExecutionEmpTerm").Item2;
                    PositionDefaultDimensionExecutionID = worker.GetPropValue("WorkerPositionFinancialDimension.ExecutionIdPositionFD").Item2;
                    PositionHierarchyExecutionID = worker.GetPropValue("WorkerPositionHierarchy.ExecutionIdPositionHierarchy").Item2;
                    PositionV2ExecutionID = worker.GetPropValue("WorkerPosition.ExecutionIdPosition").Item2;
                    PositionWorkerAssignmentExecutionID = worker.GetPropValue("WorkerPositionAssignment.ExecutionIdPositionAssign").Item2;
                    break;
                case "Employments":
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "Positions":
                    positionID = worker.GetPropValue("PositionId").Item2;
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = worker.GetPropValue("ExecutionIdPosition").Item2;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "PositionFinancialDimensions":
                    positionID = worker.GetPropValue("PositionId").Item2;
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = worker.GetPropValue("ExecutionIdPositionFD").Item2;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "EmploymentDetails":
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = worker.GetPropValue("ExecutionIdEmpDetail").Item2;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "EmploymentTerms":
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = worker.GetPropValue("ExecutionEmpTerm").Item2;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "PositionHierarchy":
                    positionID = worker.GetPropValue("PositionId").Item2;
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = worker.GetPropValue("ExecutionIdPositionHierarchy").Item2;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "EmployeeDetails":
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = worker.GetPropValue("ExecutionID").Item2;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
                case "PositionAssignments":
                    positionID = worker.GetPropValue("PositionId").Item2;
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = worker.GetPropValue("ExecutionIdPostionAssign").Item2;
                    break;
                default:
                    workerExecutionID = string.Empty;
                    EmploymentDetailExecutionID = string.Empty;
                    EmploymentEmployeeExecutionID = string.Empty;
                    EmploymentTermExecutionID = string.Empty;
                    PositionDefaultDimensionExecutionID = string.Empty;
                    PositionHierarchyExecutionID = string.Empty;
                    PositionV2ExecutionID = string.Empty;
                    PositionWorkerAssignmentExecutionID = string.Empty;
                    break;
            }


            using (SqlConnection conn = new SqlConnection(SQLConn.ConnStr()))
            {
                StringBuilder sb = new StringBuilder();

                //Reseting for WorkerStagingHistoryTable
                sb.Append($"update [{Constants.TableSchema}].[{Constants.WorkerStagingHistoryTable}] " +
                   $" set TO_BE_PROCESSED = 'N' " +
                   $" Where PERSONNELNUMBER = @workerNumber" +
                   $" and EXECUTIONID = @executionId");

                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@workerNumber", workerNumber);
                    command.Parameters.AddWithValue("@executionId", workerExecutionID);

                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                //Reseting for EmploymentDetailStagingHistoryTable

                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.EmploymentDetailStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where PERSONNELNUMBER = @workerNumber" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@workerNumber", workerNumber);
                    command.Parameters.AddWithValue("@executionId", EmploymentDetailExecutionID);
                    await command.ExecuteNonQueryAsync();
                }

                //Reseting for EmploymentEmployeeStagingHistoryTable


                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.EmploymentEmployeeStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where PERSONNELNUMBER = @workerNumber" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@workerNumber", workerNumber);
                    command.Parameters.AddWithValue("@executionId", EmploymentEmployeeExecutionID);
                    await command.ExecuteNonQueryAsync();
                }


                //Reseting for EmploymentTermStagingHistoryTable
                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.EmploymentTermStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where PERSONNELNUMBER = @workerNumber" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@workerNumber", workerNumber);
                    command.Parameters.AddWithValue("@executionId", EmploymentTermExecutionID);
                    await command.ExecuteNonQueryAsync();
                }

                //Reseting for PositionDefaultDimensionStagingHistoryTable
                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.PositionDefaultDimensionStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where POSITIONID = @positionId" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@positionId", positionID);
                    command.Parameters.AddWithValue("@executionId", PositionDefaultDimensionExecutionID);
                    await command.ExecuteNonQueryAsync();
                }



                //Reseting for PositionHierarchyStagingHistoryTable
                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.PositionHierarchyStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where POSITIONID = @positionId" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@positionId", positionID);
                    command.Parameters.AddWithValue("@executionId", PositionHierarchyExecutionID);
                    await command.ExecuteNonQueryAsync();
                }

                //Reseting for PositionV2StagingHistoryTable
                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.PositionV2StagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where POSITIONID = @positionId" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@positionId", positionID);
                    command.Parameters.AddWithValue("@executionId", PositionV2ExecutionID);
                    await command.ExecuteNonQueryAsync();
                }

                //Reseting for PositionWorkerAssignmentStagingHistoryTable
                sb.Clear();
                sb.Append($"update [{Constants.TableSchema}].[{Constants.PositionWorkerAssignmentStagingHistoryTable}] " +
                    $" set TO_BE_PROCESSED = 'N' " +
                    $" Where POSITIONID = @positionId" +
                    $" and EXECUTIONID = @executionId");
                using (SqlCommand command = new SqlCommand(sb.ToString(), conn))
                {
                    command.Parameters.AddWithValue("@positionId", positionID);
                    command.Parameters.AddWithValue("@executionId", PositionWorkerAssignmentExecutionID);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
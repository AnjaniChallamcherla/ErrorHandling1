using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace TalentErrorHandling
{
    public static class ValidateWorkers
    {
        [FunctionName("ValidateWorkers")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            
            string apiResult = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "apioutput", true) == 0)
                .Value;

            if (apiResult == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                apiResult = data?.apioutput;
            }
            
            if (!string.IsNullOrEmpty(apiResult))
            {
                string output = string.Empty;
                
                //Deserializing the json mandatoryFieldsCheck
                List<object> deserializeAPIResult = JsonConvert.DeserializeObject<List<object>>(apiResult);

                CheckWorkers checkWorkers = new CheckWorkers();

                for (int i = 0; i < deserializeAPIResult.Count; i+=2)
                {
                    switch (deserializeAPIResult[i])
                    {
                        case "Workers":
                            List<Worker> workers = JsonConvert.DeserializeObject<List<Worker>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (Worker worker in workers)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(worker, "Workers");
                            }
                            break;
                        case "Employments":
                            List<Employment> employments = JsonConvert.DeserializeObject<List<Employment>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (Employment employment in employments)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(employment, "Employments");
                            }
                            break;
                        case "Positions":
                            List<Position> positions = JsonConvert.DeserializeObject<List<Position>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (Position position in positions)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(position, "Positions");
                            }
                            break;
                        case "PositionFinancialDimensions":
                            List<PositionFinancialDimension> positionFinancialDimensions = JsonConvert.DeserializeObject<List<PositionFinancialDimension>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (PositionFinancialDimension positionFinacialDim in positionFinancialDimensions)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(positionFinacialDim, "PositionFinancialDimensions");
                            }
                            break;
                        case "EmploymentDetails":
                            List<Termination> terminations = JsonConvert.DeserializeObject<List<Termination>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (Termination termination in terminations)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(termination, "EmploymentDetails");
                            }
                            break;
                        case "EmploymentTerms":
                            List<EmploymentTerm> employmentTerms = JsonConvert.DeserializeObject<List<EmploymentTerm>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (EmploymentTerm employmentTerm in employmentTerms)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(employmentTerm, "EmploymentTerms");
                            }
                            break;
                        case "PositionHierarchy":
                            List<PositionHierarchy> positionHierarchies = JsonConvert.DeserializeObject<List<PositionHierarchy>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (PositionHierarchy positionHierarchie in positionHierarchies)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(positionHierarchie, "PositionHierarchy");
                            }
                            break;
                        case "EmployeeDetails":
                            List<EmployeeDetail> employeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetail>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (EmployeeDetail employeeDetail in employeeDetails)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(employeeDetail, "EmployeeDetails");
                            }
                            break;
                        case "PositionAssignments":
                            List<PositionAssignment> positionAssignments = JsonConvert.DeserializeObject<List<PositionAssignment>>(JsonConvert.SerializeObject(deserializeAPIResult[++i]));
                            foreach (PositionAssignment positionAssignment in positionAssignments)
                            {
                                output = await checkWorkers.ReadPropertiesRecursiveAsync(positionAssignment, "PositionAssignments");
                            }
                            break;

                    }
                }
                return req.CreateResponse(HttpStatusCode.OK, $"{output}");
            }
            return req.CreateResponse(HttpStatusCode.BadRequest, "Something seems to gave gone wrong...");
        }
    }
}
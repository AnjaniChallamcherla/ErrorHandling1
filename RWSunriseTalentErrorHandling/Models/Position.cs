using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class Position
    {
        public string PositionId { get; set; }
        public string WorkerNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public string CompensationRegionId { get; set; }
        public string DivisionCustom { get; set; }
        public byte OnsiteCustom { get; set; }
        public string FrontOfficeCustom { get; set; }
        public string JobFunctionCustom { get; set; }
        public string TitleId { get; set; }
        public string ExecutionIdPosition { get; set; }
        public string PartitionPosition { get; set; }

        public string LocationName { get; set; }
        public string DepartmentName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class PositionAssignment
    {
        public string PositionId { get; set; }
        public string WorkerNumber { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int IsPositionPrimary { get; set; }
        public string ReaonCodedId { get; set; }
        public string ExecutionIdPositionAssign { get; set; }
        public string PartitionPositionAssign { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TalentErrorHandling
{
    public class PositionHierarchy
    {
        public string PositionId { get; set; }
        public string WorkerNumber { get; set; }
        public string ParentPositionId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string HierarchyTypeName { get; set; }
        public string ExecutionIdPositionHierarchy { get; set; }
        public string PartitionPositionHierarchy { get; set; }

    }
}

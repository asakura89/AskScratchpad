using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSScratchpad.Script {
    class FormatJson : Common, IRunnable {
        public void Run() => Console.WriteLine(JsonConvert.SerializeObject(GetApprLvs(), Formatting.Indented));

        public sealed class WfApprovalLevelViewModel {
            public Int32 DataNo { get; set; }
            public String App { get; set; }
            public Int32 Segment { get; set; }
            public Int32 SegmentLevel { get; set; }
            public Int32 ApprovalLevel1 { get; set; }
            public Int32 ApprovalLevel2 { get; set; }
            public String Desc { get; set; }
            public Boolean ApprovalLevelSLA { get; set; }
            public Int32 SLA { get; set; }
        }

        IList<WfApprovalLevelViewModel> GetApprLvs() => new List<WfApprovalLevelViewModel> {
            new WfApprovalLevelViewModel {App = "ApprovalApps", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 1, ApprovalLevel2 = 1, Desc = "Staff", ApprovalLevelSLA = false, SLA = 0},
            new WfApprovalLevelViewModel {App = "ApprovalApps", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 2, ApprovalLevel2 = 3, Desc = "Manager", ApprovalLevelSLA = true, SLA = 7},
            new WfApprovalLevelViewModel {App = "ApprovalApps", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 3, ApprovalLevel2 = 1, Desc = "Senior Manager", ApprovalLevelSLA = true, SLA = 3},
            new WfApprovalLevelViewModel {App = "ApprovalApps", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 4, ApprovalLevel2 = 1, Desc = "CEO", ApprovalLevelSLA = true, SLA = 4},
            new WfApprovalLevelViewModel {App = "ApprovalApps", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 5, ApprovalLevel2 = 1, Desc = "BOD", ApprovalLevelSLA = true, SLA = 3}
        };
    }
}

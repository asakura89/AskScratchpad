#tool nuget:?package=Newtonsoft.Json&version=10.0.3
#r ../tools/newtonsoft.json.10.0.3/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll

using System;

Task("Main")
    .Does(() => {
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(GetApprLvs(), Newtonsoft.Json.Formatting.Indented));

        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    })
    .ReportError(ex => Error(ex.Message));
RunTarget("Main");


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

IList<WfApprovalLevelViewModel> GetApprLvs() {
    return new List<WfApprovalLevelViewModel> {
        new WfApprovalLevelViewModel {App = "BMS_EOA", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 1, ApprovalLevel2 = 1, Desc = "STAFF", ApprovalLevelSLA = false, SLA = 0},
        new WfApprovalLevelViewModel {App = "BMS_EOA", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 2, ApprovalLevel2 = 3, Desc = "DH", ApprovalLevelSLA = true, SLA = 7},
        new WfApprovalLevelViewModel {App = "BMS_EOA", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 3, ApprovalLevel2 = 1, Desc = "SGM", ApprovalLevelSLA = true, SLA = 3},
        new WfApprovalLevelViewModel {App = "BMS_EOA", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 4, ApprovalLevel2 = 1, Desc = "BOD", ApprovalLevelSLA = true, SLA = 4},
        new WfApprovalLevelViewModel {App = "BMS_EOA", Segment = 1, SegmentLevel = 1, ApprovalLevel1 = 5, ApprovalLevel2 = 1, Desc = "VPD", ApprovalLevelSLA = true, SLA = 3}
    };
}
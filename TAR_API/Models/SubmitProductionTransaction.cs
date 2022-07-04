using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR_API.Models
{
    public class SubmitProductionTransaction
    {
        public int PHMID { get; set; }
        public String AccountIDs { get; set; }
        public String AdditionalCapture { get; set; }
        public int ScenarioMappingID { get; set; }
        public int CallTypeID { get; set; }
        public string Note { get; set; }
        public string SoftwareNotes { get; set; }
        public int UserID { get; set; }
        public bool IsTempSave { get; set; }
        public string DeferDate { get; set; }
        public String TimeTakenJSON { get; set; }
        public string RoleCode { get; set; }
    }

    public class SubmitSMETransaction
    {
        public int PHMID { get; set; }
        public String AccountIDs { get; set; }
        public String AdditionalCapture { get; set; }
        public int ScenarioMappingID { get; set; }
        public int CallTypeID { get; set; }
        public string Note { get; set; }
        public string SoftwareNotes { get; set; }
        public int UserID { get; set; }
        public bool IsTempSave { get; set; }
        public string DeferDate { get; set; }
        public String TimeTakenJSON { get; set; }
        public string RoleCode { get; set; }
    }

    public class SubmitSUpplyEntryTransaction
    {
        public int PHMID { get; set; }
        public int UserID { get; set; }
        public String AccountIDs { get; set; }
        public string DistinctID { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string RoleCode { get; set; }
        public String AdditionalCapture { get; set; }
        public string SoftwareNotes { get; set; }
        public int ScenarioMappingID { get; set; }
        public int CallTypeID { get; set; }
        public bool IsTempSave { get; set; }
        public string DeferDate { get; set; }
        public int RuleID { get; set; }
        // public String TimeTakenJSON { get; set; }

    }
}
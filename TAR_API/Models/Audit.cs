using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR_API.Models
{
    public class Audit
    {
        public string StatusCode { get; set; }
        public int PHMID { get; set; }
        public int ImportFileID { get; set; }
        public int UserID { get; set; }
        public int DispositionID { get; set; }
        public int SubDispositionID { get; set; }
        public int ActionCodeID { get; set; }
        public string Associate { get; set; }
        public string Payer { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AssociateKeyNoteSearch { get; set; }
        public int CallTypeID  { get; set; }
    }

    public class SubmitAuditTransaction
    {
        public String AuditList { get; set; }
        public int PHMID { get; set; }
        public string CorrectiveNote { get; set; }

        public String ErrorList { get; set; }
        public String ErrorMappingID { get; set; }
        public String AccountIDs { get; set; }

        public string Note { get; set; }

        public int UserID { get; set; }
        public bool IsReject { get; set; }
        public bool IsBulk { get; set; }
        public String TimeTakenJSON { get; set; }




    }
    public class EditAuditTransaction
    {
        public int PHMID { get; set; }
        public String AccountIDs { get; set; }
        public string AssociateNotes { get; set; }
        public int UserID { get; set; }

        public int DispositionID { get; set; }
        public int SubDispositionID { get; set; }
        public int ActionCode { get; set; }
        public int CallType { get; set; }




    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace TDADOAMSSETEDF.Models
{
    public class ApprovalHistory
    {
        public string Approver { get; set; }
        public string Decision { get; set; }
        public string Comment { get; set; }
        public DateTime DecisionDate { get; set; }

        public ApprovalHistory(string approver, string decision, string comment)
        {
            Approver = approver;
            Decision = decision;
            Comment = comment;
            DecisionDate = DateTime.Now;
        }
    }
}

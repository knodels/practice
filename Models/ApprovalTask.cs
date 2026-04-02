using System;
using System.Collections.Generic;
using System.Text;

namespace TDADOAMSSETEDF.Models
{
    public class ApprovalTask
    {
        public int DocumentId { get; set; }
        public List<string> Approvers { get; set; }
        public int CurrentApproverIndex { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public List<ApprovalHistory> History { get; set; }

        public ApprovalTask(int documentId, List<string> approvers, int deadlineHours)
        {
            DocumentId = documentId;
            Approvers = approvers ?? new List<string>();
            CurrentApproverIndex = 0;
            Status = ApprovalStatus.Pending;
            Deadline = DateTime.Now.AddHours(deadlineHours);
            History = new List<ApprovalHistory>();
        }
    }
    public enum ApprovalStatus
    {
        Pending, // ожидание
        InProgress, // в процессе
        Approved, // соглашен
        Rejected, // откланен
        Overdue // просрочен  
    }
}

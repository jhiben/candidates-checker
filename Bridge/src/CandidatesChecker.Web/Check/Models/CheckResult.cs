using System;

namespace CandidatesChecker.Web.Check.Models
{
    public class CheckResult
    {
        public bool IsContacted => ContactedOn.HasValue;

        public DateTime? ContactedOn { get; }

        public string? ContactedBy { get; set; }

        public CheckResult()
        {
        }

        public CheckResult(DateTime contactedOn, string contactedBy)
        {
            ContactedOn = contactedOn;
            ContactedBy = contactedBy;
        }
    }
}

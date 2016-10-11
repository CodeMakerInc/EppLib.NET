using System;

namespace EppLib.Extensions.LaunchPhase.ClaimCreate
{
    public class ClaimNotice
    {
        public string NoticeId { get; set; }
        public string ValidatorId { get; set; }
        public DateTime NotAfter { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}

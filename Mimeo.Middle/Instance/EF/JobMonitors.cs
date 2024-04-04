using System;
using System.Collections.Generic;

namespace Mimeo.Middle.Instance.EF
{
    public partial class JobMonitors
    {
        public long Id { get; set; }
        public long InstanceId { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Description { get; set; }
        public string HangFireJobId { get; set; }
        public bool ReceivedKillSignal { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Instances Instance { get; set; }
    }
}

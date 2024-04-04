using System;
using System.Collections.Generic;

namespace Mimeo.Middle.Instance.EF
{
    public partial class Instances
    {
        public Instances()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            JobMonitors = new HashSet<JobMonitors>();
        }

        public long Id { get; set; }
        public string InstanceName { get; set; }
        public string Database { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<JobMonitors> JobMonitors { get; set; }
    }
}

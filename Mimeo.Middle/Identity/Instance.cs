using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mimeo.Middle.Identity
{
    public class Instance
    {
        [Key]
        public long Id { get; set; }
        public string FriendlyId => $"{this.Id.ToString().PadLeft(4, '0')}";
        public string InstanceName { get; set; }
        public string Database { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual List<ApplicationUser> ApplicationUsers { get; set; }

        public override string ToString()
        {
            return $"Instance {FriendlyId} - {InstanceName} - {Database}";
        }
    }
}


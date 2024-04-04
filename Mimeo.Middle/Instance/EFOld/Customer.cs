using System;

namespace Mimeo.Middle.Instance.EFOld
{
    public partial class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? MainAddressId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual EFOld.Address MainAddress { get; set; }
    }
}

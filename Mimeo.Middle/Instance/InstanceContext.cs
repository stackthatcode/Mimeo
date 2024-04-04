using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimeo.Middle.Identity;
using Mimeo.Middle.Instance.EF;

namespace Mimeo.Middle.Instance
{
    public class InstanceContext
    {
        // Identity/Master DB-wide data access
        //
        private readonly IdentityService _identityService;

        // Uniquely identifies this object
        //
        private readonly Guid _contextId;

        // Instance-specific data access piggybacks on the Instance Context lifetime scope
        //
        private long _instanceId = -1;
        private MimeoInstanceDbContext _instanceDbContext;

        public InstanceContext(IdentityService identityService)
        {
            _contextId = Guid.NewGuid();
            _identityService = identityService;
        }

        public MimeoInstanceDbContext InstanceDbContext => _instanceDbContext;
        public bool IsInitialized => InstanceId != -1;
        public long InstanceId
        {
            get
            {
                if (!IsInitialized)
                {
                    throw new Exception("Instance Id is not set because Initialize has not been called.");
                }

                return _instanceId;
            }
        }


        public async Task<bool> InitializeAsync(long instanceId)
        {
            _instanceId = instanceId;

            var instance = await _identityService.RetrieveInstance(instanceId);
            var connectionsString = ConnectionStringBuilder.Build(instance.Database);

            var options
                = new DbContextOptionsBuilder<MimeoInstanceDbContext>()
                    .UseSqlServer(connectionsString)
                    .Options;

            _instanceDbContext = new MimeoInstanceDbContext(options);
            return true;
        }
    }
}


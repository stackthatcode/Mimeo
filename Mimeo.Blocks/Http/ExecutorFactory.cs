using System;

namespace Mimeo.Blocks.Http
{
    public class ExecutorFactory
    {
        private readonly Func<FaultTolerantExecutor> _executorMaker;

        public ExecutorFactory(Func<FaultTolerantExecutor> executorMaker)
        {
            _executorMaker = executorMaker;
        }

        public FaultTolerantExecutor Make(
                int maxNumberOfAttempts = 1, int throttlingDelay = 0, string throttlingKey = null)
        {
            // Invoke the Autofac factory-by-convention
            //
            var output = _executorMaker();
            output.MaxNumberOfAttempts = maxNumberOfAttempts;
            output.ThrottlingDelay = throttlingDelay;

            if (throttlingKey != null)
            {
                output.ThrottlingKey = throttlingKey;
            }

            return output;
        }
    }
}

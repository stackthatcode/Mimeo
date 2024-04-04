using System;
using System.Threading.Tasks;

namespace Mimeo.Blocks.Async
{
    public static class Helper
    {
        [Obsolete("Please use Stephen Cleary's Nito.AsyncEx -> AsyncContext object for this")]
        public static void ExecuteSync(this Func<Task> operation)
        {
            var task = Task.Run(async () =>
            {
                await operation();
            });
            task.Wait();
        }

    }
}

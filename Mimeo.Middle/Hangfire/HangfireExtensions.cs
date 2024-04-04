using Hangfire.Storage;
using Mimeo.Blocks.Helpers;
using Mimeo.Middle.Instance.EF;

namespace Mimeo.Middle.Hangfire
{
    public static class HangfireExtensions
    {
        public static bool IsAlive(this string jobState)
        {
            if (jobState == "Succeeded" || jobState == "Failed" || jobState == "Deleted")
            {
                return false;
            }

            return true;
        }

        public static bool IsAlive(this JobData jobData)
        {
            if (jobData == null)
            {
                return false;
            }
            else
            {
                return jobData.State.IsAlive();
            }
        }

        public static string FullDescription(this JobMonitors input)
        {
            return
                $"Job (Id: {input.Id} / HangfireId: {input.HangFireJobId.IsNullOrEmptyAlt("[not set]")})" +
                $" - {input.Description} - RecvdKillSignal: {input.ReceivedKillSignal.ToYesNo()}";
        }
    }
}


using System;

namespace Mimeo.Middle.Hangfire
{
    public class MainProgram
    {
        public void Execute()
        {
            var scheduler = new Scheduler();
            scheduler.InstanceId = 1285980801;

            scheduler.Schedule<BackgroundWorker>(x => x.DoWork());
        }

    }


    public class Scheduler
    {
        public long InstanceId { get; set; }

        public void Schedule<T>(Action<T> action) where T : new()
        {
            var monitorId = 123;
            var worker = new T();
            var runner = new JobRunnerTest<T>(worker);
            runner.Execute(InstanceId, monitorId, action);
        }
    }



    public class JobRunnerTest<T> where T: new()
    {
        private readonly T _worker;

        // Obviously replace this with DI framework
        //
        public JobRunnerTest(T worker)
        {
            _worker = worker;
        }

        public void InitializeContext(long instanceId)
        {
            // Do stuff, like load Identity, blah blah
            //
        }

        public void Execute(long instanceId, long monitorId, Action<T> action)
        {
            InitializeContext(instanceId);

            // Do your Instance Context stuff here, brah...
            //
            action(_worker);
        }
    }

    public class BackgroundWorker
    {
        public void DoWork()
        {
        }
    }
}


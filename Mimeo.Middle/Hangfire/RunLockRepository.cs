using System;
using System.Collections.Concurrent;
using Mimeo.Blocks.Logging;

namespace Mimeo.Middle.Hangfire
{
    public class RunLockRepository
    {
        private static readonly
                ConcurrentDictionary<string, string> _storage = new ConcurrentDictionary<string, string>();

        private static readonly object _lock = new object();

        private readonly MimeoLogger _logger;

        public RunLockRepository(MimeoLogger logger)
        {
            _logger = logger;
        }


        public bool Acquire(string uniqueIdentifier)
        {
            lock (_lock)
            {
                if (_storage.ContainsKey(uniqueIdentifier))
                {
                    var msg = $"Failed to acquire Run Lock for {uniqueIdentifier}";
                    _logger.Info(msg);

                    return false;
                }
                else
                {
                    _storage[uniqueIdentifier] = uniqueIdentifier;
                    var msg = $"Successfully acquired Run Lock for {uniqueIdentifier}";
                    _logger.Info(msg);
                    return true;
                }
            }
        }

        public bool Free(string uniqueIdentifier)
        {
            lock (_lock)
            {
                try
                {
                    string keyOut;
                    _storage.TryRemove(uniqueIdentifier, out keyOut);

                    var msg = $"Freed Run Lock for {uniqueIdentifier}";
                    _logger.Info(msg);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}


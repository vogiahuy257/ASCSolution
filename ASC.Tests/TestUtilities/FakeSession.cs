using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ASC.Tests.TestUtilities
{
    public class FakeSession : ISession
    {
        private Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => sessionStorage.Keys;

        public void Clear()
        {
            sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return sessionStorage.TryGetValue(key, out value);
        }
    }
}

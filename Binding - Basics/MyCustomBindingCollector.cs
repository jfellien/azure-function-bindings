using Microsoft.Azure.WebJobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyCustomBinding
{
    /// <summary>
    /// Stellt virtuell die Verbindung zwischen Code und Server dar
    /// </summary>
    public class MyCustomBindingCollector : IAsyncCollector<string>
    {
        public MyCustomBindingCollector(MyCustomBindingAttribute attribute)
        {

        }
        public Task AddAsync(string item, CancellationToken cancellationToken = default)
        {
            // Hier werden die Daten weiterverarbeitet, z.B. in einer Datenbank gespeichert
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

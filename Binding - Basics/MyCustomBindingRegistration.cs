using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using MyCustomBinding;

[assembly: WebJobsStartup(typeof(MyCustomBindingRegistration))]

namespace MyCustomBinding
{
    public class MyCustomBindingRegistration : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<MyCustomBindingConfiguration>();
        }
    }
}

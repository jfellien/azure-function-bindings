using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCustomBinding
{
    /// <summary>
    /// Hält Konfigurationsdaten für das Binding bereit 
    /// und wird zur Markierung von Parametern verwendet
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class MyCustomBindingAttribute : Attribute
    {
        [AutoResolve]
        public string SomeSettings { get; set; }
    }
}

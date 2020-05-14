using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace MyCustomBinding
{
    /// <summary>
    /// Definiert/Konfiguriert das Binding. Legt die Regeln an, wie das Binding erfolgt.
    /// </summary>
    [Extension("MyCustomBinding")]
    public class MyCustomBindingConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<MyCustomBindingAttribute>()
                .BindToCollector(attribute => new MyCustomBindingCollector(attribute));
            
            context.AddBindingRule<MyCustomBindingAttribute>()
                .BindToInput(attribute => {
                    // verwenden der Werte von attribute, um einen Wert zu ermitteln

                    return "Wert, der an die Funktion übergeben wird";
                });
        }
    }
}

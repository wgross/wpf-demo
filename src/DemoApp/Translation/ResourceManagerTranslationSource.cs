using System.Resources;

namespace DemoApp.Translation
{
    /// <summary>
    /// Implement a translation source which is added to the resources of a translated control.
    /// </summary>
    public sealed class ResourceManagerTranslationSource : ITranslationSource
    {   
        public ResourceManager ResourceManager
        {
            get; set;
        }

        public string GetString(string key) => this.ResourceManager.GetString(key);
    }
}
using System.Resources;

namespace DemoApp.Translation
{
    public sealed class ResourceManagerTranslationSource : ITranslationSource
    {
        public ResourceManager ResourceManager
        {
            get; set;
        }

        public string GetString(string key) => this.ResourceManager.GetString(key);
    }
}
using DemoApp.Translation;
using Microsoft.Extensions.Logging;

namespace DemoApp
{
    public class MainWindowViewModel
    {
        private readonly ILogger<MainWindowViewModel> logger;

        public MainWindowViewModel(TranslationExampleViewModel trvm, ILogger<MainWindowViewModel> logger)
        {
            this.logger = logger;
            this.logger.LogDebug("MainWindowViewModel created");
            this.TranslationExample = trvm;
        }

        public TranslationExampleViewModel TranslationExample { get; private set; }
    }
}
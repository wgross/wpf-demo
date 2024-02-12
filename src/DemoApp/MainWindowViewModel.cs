using DemoApp.ListTree;
using DemoApp.Translation;
using DemoApp.Validation;
using Microsoft.Extensions.Logging;

namespace DemoApp;

public class MainWindowViewModel
{
    private readonly ILogger<MainWindowViewModel> logger;

    public MainWindowViewModel(
        TranslationExampleViewModel trvm,
        ValidationExampleViewModel vlvm,
        ListTreeExampleViewModel ltvm,
        ILogger<MainWindowViewModel> logger)
    {
        this.logger = logger;
        this.logger.LogDebug("MainWindowViewModel created");
        this.TranslationExample = trvm;
        this.ValidationExample = vlvm;
        this.ListTreeExample = ltvm;
    }

    public TranslationExampleViewModel TranslationExample { get; private set; }

    public ValidationExampleViewModel ValidationExample { get; }

    public ListTreeExampleViewModel ListTreeExample { get; }
}
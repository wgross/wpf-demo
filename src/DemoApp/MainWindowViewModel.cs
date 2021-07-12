using Microsoft.Extensions.Logging;

namespace DemoApp
{
    public class MainWindowViewModel
    {
        private readonly ILogger<MainWindowViewModel> logger;

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger)
        {
            this.logger = logger;
        }
    }
}
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DemoApp.Translation
{
    public record SelectCultureItem(string Name, CultureInfo Culture);

    public class TranslationExampleViewModel : ObservableObject
    {
        private readonly CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

        private SelectCultureItem selectedCulture;

        public TranslationExampleViewModel()
        {
            this.Cultures = new ObservableCollection<SelectCultureItem>
            {
                new("Deutsch", this.cultures.First(c => c.Name.StartsWith("de"))),
                new("English", this.cultures.First(c => c.Name.StartsWith("en"))),
            };
            this.SelectedCulture = this.Cultures.First(c => Thread.CurrentThread.CurrentUICulture.Name.StartsWith(c.Culture.Name));
        }

        public ObservableCollection<SelectCultureItem> Cultures { get; set; }

        public SelectCultureItem SelectedCulture
        {
            get => this.selectedCulture;
            set
            {
                if (this.SetProperty(ref this.selectedCulture, value))
                {
                    // send the global event to refresh all translations
                    UICultureChangedEvent.SetCurrentUICulture(value.Culture);
                }
            }
        }
    }
}
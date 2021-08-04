using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Validation
{
    public class ValidationExampleViewModel : ObservableValidator
    {
        private string text;

        [Required]
        [MaxLength(10)]
        public string Text
        {
            set => this.SetProperty(ref this.text, value, validate: true);
            get => this.text;
        }
    }
}
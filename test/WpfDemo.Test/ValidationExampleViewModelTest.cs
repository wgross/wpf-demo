using DemoApp.Validation;
using System.Linq;
using Xunit;

namespace DempApp.Test
{
    public class UnitTest1
    {
        private ValidationExampleViewModel viewModel;

        public UnitTest1()
        {
            this.viewModel = new ValidationExampleViewModel();
        }

        [Fact]
        public void Text_is_required()
        {
            // ACT
            this.viewModel.Text = string.Empty;

            // ASSERT
            Assert.Equal(string.Empty, this.viewModel.Text);
            Assert.Equal("The Text field is required.", this.viewModel.GetErrors(nameof(this.viewModel.Text)).Single().ErrorMessage);
        }

        [Fact]
        public void Text_is_shorter_than_10()
        {
            // ACT
            this.viewModel.Text = "01234567891";

            // ASSERT
            Assert.Equal("01234567891", this.viewModel.Text);
            Assert.Equal("The field Text must be a string or array type with a maximum length of '10'.", this.viewModel.GetErrors(nameof(this.viewModel.Text)).Single().ErrorMessage);
        }
    }
}
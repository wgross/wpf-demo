using System;
using System.ComponentModel;
using Xunit;

namespace DemoApp.Test
{
    public class PropertyBagViewModelTest
    {
        private class TestViewModel : PropertyBagViewModelBase
        {
            public int Property
            {
                get => this.GetPropertyValue<int>();
                set => this.SetPropertyValue(value);
            }

            internal void SetWithPropertyName(string propertyName) => this.SetPropertyValue(1, propertyName);

            internal object GetWithPropertyName(string propertyName) => this.GetPropertyValue<object>(propertyName);
        }

        [Fact]
        public void Setting_property_value_notifies()
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            (object sender, PropertyChangingEventArgs args) result_before = default;
            (object sender, PropertyChangedEventArgs args) result_after = default;

            // ACT
            void propertyChanging(object sender, PropertyChangingEventArgs e) => result_before = (sender, e);
            viewModel.PropertyChanging += propertyChanging;

            void propertyChanged(object sender, PropertyChangedEventArgs e) => result_after = (sender, e);
            viewModel.PropertyChanged += propertyChanged;

            viewModel.Property = 1;

            // ASSERT
            Assert.Equal(nameof(TestViewModel.Property), result_before.args.PropertyName);
            Assert.Same(viewModel, result_before.sender);

            Assert.Equal(nameof(TestViewModel.Property), result_after.args.PropertyName);
            Assert.Same(viewModel, result_after.sender);
        }

        [Fact]
        public void Setting_property_default_value_notifies()
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            (object sender, PropertyChangingEventArgs args) result_before = default;
            (object sender, PropertyChangedEventArgs args) result_after = default;

            // ACT
            void propertyChanging(object sender, PropertyChangingEventArgs e) => result_before = (sender, e);
            viewModel.PropertyChanging += propertyChanging;

            void propertyChanged(object sender, PropertyChangedEventArgs e) => result_after = (sender, e);
            viewModel.PropertyChanged += propertyChanged;

            viewModel.Property = 0;

            // ASSERT
            Assert.Equal(nameof(TestViewModel.Property), result_before.args.PropertyName);
            Assert.Same(viewModel, result_before.sender);

            Assert.Equal(nameof(TestViewModel.Property), result_after.args.PropertyName);
            Assert.Same(viewModel, result_after.sender);
        }

        [Fact]
        public void Setting_same_property_value_twice_property_doesnt_notify()
        {
            // ARRANGE
            var viewModel = new TestViewModel();
            viewModel.Property = 1;

            (object sender, PropertyChangingEventArgs args) result_before = default;
            (object sender, PropertyChangedEventArgs args) result_after = default;

            // ACT
            void propertyChanging(object sender, PropertyChangingEventArgs e) => result_before = (sender, e);
            viewModel.PropertyChanging += propertyChanging;

            void propertyChanged(object sender, PropertyChangedEventArgs e) => result_after = (sender, e);
            viewModel.PropertyChanged += propertyChanged;

            viewModel.Property = 1;

            // ASSERT
            Assert.Null(result_before.args);
            Assert.Null(result_before.sender);

            Assert.Null(result_after.args);
            Assert.Null(result_after.sender);
        }

        [Theory]
        [InlineData("")]
        [InlineData((string)null)]
        public void Reject_setting_without_propertyname(string propertyName)
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            // ACT
            var result = Assert.Throws<ArgumentNullException>(() => viewModel.SetWithPropertyName(propertyName));

            // ASSERT
            Assert.Equal("callerMemberName", result.ParamName);
        }

        [Fact]
        public void Read_property_value()
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            viewModel.Property = 1;

            // ACT
            var result = viewModel.Property;

            // ASSERT
            Assert.Equal(1, result);
        }

        [Fact]
        public void Read_property_value_before_setting_returns_default()
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            // ACT
            var result = viewModel.Property;

            // ASSERT
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData((string)null)]
        public void Reject_getting_without_propertyname(string propertyName)
        {
            // ARRANGE
            var viewModel = new TestViewModel();

            // ACT
            var result = Assert.Throws<ArgumentNullException>(() => viewModel.GetWithPropertyName(propertyName));

            // ASSERT
            Assert.Equal("callerMemberName", result.ParamName);
        }
    }
}
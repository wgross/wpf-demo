using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DemoApp.PolymorphTab;

public class TabItemViewModelA() : ObservableObject
{
    public string Name { get; } = "an A";
}

public class TabItemViewModelB : ObservableObject
{
    private readonly PolymorphTabViewModel polymorphTabViewModel;

    public TabItemViewModelB(PolymorphTabViewModel polymorphTabViewModel)
    {
        this.polymorphTabViewModel = polymorphTabViewModel;
        this.AddNewACommand = new RelayCommand(() =>
        {
            this.polymorphTabViewModel.Items.Insert(this.polymorphTabViewModel.Items.Count - 1, new TabItemViewModelA());
        });
    }

    public string Name { get; } = "Add new A";

    public RelayCommand AddNewACommand { get; }
}

public class PolymorphTabViewModel : ObservableObject
{
    public PolymorphTabViewModel()
    {
        this.Items = new ObservableCollection<ObservableObject> { new TabItemViewModelA(), new TabItemViewModelB(this) };
    }

    public ObservableCollection<ObservableObject> Items { get; }
}
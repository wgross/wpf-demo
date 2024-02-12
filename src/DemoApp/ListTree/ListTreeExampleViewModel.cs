using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DemoApp.ListTree;

public class LevelItemViewModel : ObservableObject
{
    public string Name { get; init; }
}


public class LevelViewModel : ObservableObject
{
    public string Name { get; init; }

    public int Index { get; init; }

    public ObservableCollection<LevelItemViewModel> Items { get; } = new ObservableCollection<LevelItemViewModel>()
    {
        new LevelItemViewModel{Name="one"},

        new LevelItemViewModel{Name="two" }
    };
}

public class ListTreeExampleViewModel : ObservableObject
{
    public ObservableCollection<LevelViewModel> Levels { get; } = new ObservableCollection<LevelViewModel>()
    {
        new LevelViewModel{Name="one", Index=0 },

        new LevelViewModel{Name="two", Index=1 }
    };
}
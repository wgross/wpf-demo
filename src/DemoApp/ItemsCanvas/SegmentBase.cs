using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.ItemsCanvas;

public abstract partial class SegmentBase : ObservableObject
{
    [ObservableProperty]
    private double left;

    [ObservableProperty]
    private double top;
}
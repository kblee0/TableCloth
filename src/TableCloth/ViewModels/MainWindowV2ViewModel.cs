﻿using System;
using TableCloth.Commands.MainWindowV2;

namespace TableCloth.ViewModels;

[Obsolete("This class is reserved for design-time usage.", false)]
public class MainWindowV2ViewModelForDesigner : MainWindowV2ViewModel { }

public class MainWindowV2ViewModel : ViewModelBase
{
    protected MainWindowV2ViewModel() { }

    public MainWindowV2ViewModel(
        MainWindowV2LoadedCommand mainWindowV2LoadedCommand,
        MainWindowV2ClosedCommand mainWindowClosedCommand)
    {
        _mainWindowV2LoadedCommand = mainWindowV2LoadedCommand;
        _mainWindowClosedCommand = mainWindowClosedCommand;
    }

    private readonly MainWindowV2LoadedCommand _mainWindowV2LoadedCommand = default!;
    private readonly MainWindowV2ClosedCommand _mainWindowClosedCommand = default!;

    public MainWindowV2LoadedCommand MainWindowV2LoadedCommand
        => _mainWindowV2LoadedCommand;

    public MainWindowV2ClosedCommand MainWindowClosedCommand
        => _mainWindowClosedCommand;
}

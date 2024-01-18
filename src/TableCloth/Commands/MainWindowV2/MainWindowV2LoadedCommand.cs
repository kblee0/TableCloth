﻿using System;
using System.Linq;
using System.Windows;
using TableCloth.Components;
using TableCloth.Models.Catalog;
using TableCloth.ViewModels;

namespace TableCloth.Commands.MainWindowV2;

public sealed class MainWindowV2LoadedCommand(
    IApplicationService applicationService,
    IResourceCacheManager resourceCacheManager,
    INavigationService navigationService,
    ICommandLineArguments commandLineArguments) : ViewModelCommandBase<MainWindowV2ViewModel>
{
    public override void Execute(MainWindowV2ViewModel viewModel)
    {
        applicationService.ApplyCosmeticChangeToMainWindow();

        var parsedArg = commandLineArguments.Current;
        var services = resourceCacheManager.CatalogDocument.Services;

        var commandLineSelectedService = default(CatalogInternetService);
        if (parsedArg != null && parsedArg.SelectedServices.Count() > 0)
        {
            commandLineSelectedService = services
                .Where(x => parsedArg.SelectedServices.Contains(x.Id))
                .FirstOrDefault();
        }

        if (commandLineSelectedService != null)
            navigationService.NavigateToDetail(commandLineSelectedService, parsedArg);
        else
            navigationService.NavigateToCatalog(string.Empty);
    }
}

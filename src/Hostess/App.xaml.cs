﻿using Hostess.Commands;
using Hostess.Commands.AboutWindow;
using Hostess.Commands.MainWindow;
using Hostess.Commands.PrecautionsWindow;
using Hostess.Components;
using Hostess.Dialogs;
using Hostess.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TableCloth.Models;
using TableCloth.Resources;

namespace Hostess
{
    public partial class App : Application
    {
        public App()
        {
            Current.InitServiceProvider(_serviceProvider = ConfigureServices());
            InitializeComponent();
        }

        private readonly IServiceProvider _serviceProvider;
        
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var appMessageBox = _serviceProvider.GetRequiredService<AppMessageBox>();
            var commandLineArguments = _serviceProvider.GetRequiredService<CommandLineArguments>();
            var parsedArgs = commandLineArguments.Current;

            if (parsedArgs.ShowCommandLineHelp)
            {
                appMessageBox.DisplayInfo(StringResources.TableCloth_Hostess_Switches_Help, MessageBoxButton.OK);
                return;
            }

            var appStartup = _serviceProvider.GetRequiredService<AppStartup>();
            var appUserInterface = _serviceProvider.GetRequiredService<AppUserInterface>();

            var warnings = new List<string>();
            var result = await appStartup.HasRequirementsMetAsync(warnings);

            if (!result.Succeed)
            {
                appMessageBox.DisplayError(result.FailedReason, result.IsCritical);

                if (result.IsCritical)
                {
#if DEBUG
                    throw result.FailedReason ?? new Exception(StringResources.Error_Unknown());
#else
                    Shutdown(-1);
#endif
                }
            }

            if (warnings.Any())
                appMessageBox.DisplayError(string.Join(Environment.NewLine + Environment.NewLine, warnings), false);

            result = await appStartup.InitializeAsync(warnings);

            if (!result.Succeed)
            {
                appMessageBox.DisplayError(result.FailedReason, result.IsCritical);

                if (result.IsCritical)
                {
#if DEBUG
                    throw result.FailedReason ?? new Exception(StringResources.Error_Unknown());
#else
                    Shutdown(-1);
#endif
                }
            }

            var mainWindow = appUserInterface.CreateMainWindow();
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add HTTP Service
            services.AddHttpClient(nameof(Hostess), c => c.DefaultRequestHeaders.Add("User-Agent", StringResources.UserAgentText));

            // Components
            services
                .AddSingleton<AppMessageBox>()
                .AddSingleton<AppUserInterface>()
                .AddSingleton<LicenseDescriptor>()
                .AddSingleton<ProtectCriticalServices>()
                .AddSingleton<SharedProperties>()
                .AddSingleton<VisualThemeManager>()
                .AddSingleton<SharedLocations>()
                .AddSingleton<AppStartup>()
                .AddSingleton<ResourceResolver>()
                .AddSingleton<ResourceCacheManager>()
                .AddSingleton<CommandLineArguments>();

            // Shared Commands
            services
                .AddSingleton<OpenAppHomepageCommand>()
                .AddSingleton<AboutThisAppCommand>();

            // About Window
            services
                .AddWindow<AboutWindow, AboutWindowViewModel>()
                .AddSingleton<AboutWindowLoadedCommand>()
                .AddSingleton<AboutWindowCloseCommand>();

            // Precautions Window
            services
                .AddWindow<PrecautionsWindow, PrecautionsWindowViewModel>()
                .AddSingleton<PrecautionsWindowLoadedCommand>()
                .AddSingleton<PrecautionsWindowCloseCommand>();

            // Main Window
            services
                .AddWindow<MainWindow, MainWindowViewModel>()
                .AddSingleton<MainWindowLoadedCommand>()
                .AddSingleton<MainWindowInstallPackagesCommand>();

            // App
            services.AddTransient(_ => Current);

            return services.BuildServiceProvider();
        }
    }
}

﻿using Hostess.Steps.Implementations;
using Hostess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using TableCloth.Resources;

namespace Hostess.Components.Implementations
{
    public sealed class StepsComposer : IStepsComposer
    {
        public StepsComposer(
            IStepFactory stepFactory,
            ICommandLineArguments commandLineArguments,
            IResourceCacheManager resourceCacheManager)
        {
            _stepFactory = stepFactory;
            _commandLineArguments = commandLineArguments;
            _resourceCacheManager = resourceCacheManager;
        }

        private readonly IStepFactory _stepFactory;
        private readonly ICommandLineArguments _commandLineArguments;
        private readonly IResourceCacheManager _resourceCacheManager;

        public IEnumerable<StepItemViewModel> ComposeSteps()
        {
            var parsedArgs = _commandLineArguments.Current;
            var catalog = _resourceCacheManager.CatalogDocument;
            var targets = parsedArgs.SelectedServices;
            var steps = new List<StepItemViewModel>();

            steps.AddRange(new[]
            {
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(VerifyWindowsContainerEnvironmentStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Prerequisites,
                    PackageName = UIStringResources.Install_VerifyEnvironment,
                },
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(PrepareDirectoriesStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Prerequisites,
                    PackageName = UIStringResources.Install_PrepareEnvironment,
                },
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(TryProtectCriticalServicesStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Prerequisites,
                    PackageName = UIStringResources.Install_TryProtectCriticalServices,
                },
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(SetDesktopWallpaperStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Prerequisites,
                    PackageName = UIStringResources.Install_SetDesktopWallpaper,
                },
            });

            if (parsedArgs.EnableInternetExplorerMode.HasValue &&
                parsedArgs.EnableInternetExplorerMode.Value)
            {
                steps.Add(new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(EnableInternetExplorerModeStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Prerequisites,
                    PackageName = UIStringResources.Install_EnableIEMode,
                });
            }

            foreach (var eachTargetName in targets)
            {
                var targetService = catalog.Services.FirstOrDefault(x => string.Equals(eachTargetName, x.Id, StringComparison.Ordinal));

                if (targetService == null)
                    continue;

                steps.AddRange(targetService.Packages.Select(eachPackage => new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(PackageInstallStep)),
                    Argument = new PackageInstallItemViewModel
                    {
                        PackageUrl = eachPackage.Url,
                        Arguments = eachPackage.Arguments,
                    },
                    TargetSiteName = targetService.DisplayName,
                    TargetSiteUrl = targetService.Url,
                    PackageName = eachPackage.Name,
                }));

                steps.AddRange(targetService.EdgeExtensions.Select(eachEdgeExtension => new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(EdgeExtensionInstallStep)),
                    Argument = new EdgeExtensionInstallItemViewModel
                    {
                        EdgeExtensionId = eachEdgeExtension.ExtensionId,
                        EdgeCrxUrl = eachEdgeExtension.CrxUrl,
                    },
                    TargetSiteName = targetService.DisplayName,
                    TargetSiteUrl = targetService.Url,
                    PackageName = eachEdgeExtension.Name,
                }));

                var bootstrapData = targetService.CustomBootstrap;

                if (!string.IsNullOrWhiteSpace(bootstrapData))
                {
                    steps.Add(new StepItemViewModel
                    {
                        Step = _stepFactory.GetStepByName(nameof(PowerShellScriptRunStep)),
                        Argument = new PowerShellScriptInstallItemViewModel
                        {
                            ScriptContent = bootstrapData,
                        },
                        TargetSiteName = targetService.DisplayName,
                        TargetSiteUrl = targetService.Url,
                        PackageName = UIStringResources.Hostess_CustomScript_Title,
                    });
                }
            }

            steps.AddRange(new[]
            {
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(ConfigAhnLabSafeTransactionStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Config,
                    PackageName = UIStringResources.Install_ConfigASTx,
                },
                new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(ReloadEdgeStep)),
                    Argument = new InstallItemViewModel(),
                    TargetSiteName = UIStringResources.Option_Config,
                    PackageName = UIStringResources.Install_ReloadMicrosoftEdge,
                },
            });

            if (parsedArgs.InstallAdobeReader.HasValue &&
                parsedArgs.InstallAdobeReader.Value)
            {
                steps.Add(new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(OpenWebSiteStep)),
                    Argument = new OpenWebSiteItemViewModel
                    {
                        TargetUrl = CommonStrings.AdobeReaderUrl,
                    },
                    TargetSiteName = UIStringResources.Option_Addin,
                    PackageName = UIStringResources.Option_InstallAdobeReader,
                });
            }

            if (parsedArgs.InstallEveryonesPrinter.HasValue &&
                parsedArgs.InstallEveryonesPrinter.Value)
            {
                steps.Add(new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(OpenWebSiteStep)),
                    Argument = new OpenWebSiteItemViewModel
                    {
                        TargetUrl = CommonStrings.EveryonesPrinterUrl,
                    },
                    TargetSiteName = UIStringResources.Option_Addin,
                    PackageName = UIStringResources.Option_InstallEveryonesPrinter,
                });
            }

            if (parsedArgs.InstallHancomOfficeViewer.HasValue &&
                parsedArgs.InstallHancomOfficeViewer.Value)
            {
                steps.Add(new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(OpenWebSiteStep)),
                    Argument = new OpenWebSiteItemViewModel
                    {
                        TargetUrl = CommonStrings.HancomOfficeViewerUrl,
                    },
                    TargetSiteName = UIStringResources.Option_Addin,
                    PackageName = UIStringResources.Option_InstallHancomOfficeViewer,
                });
            }

            if (parsedArgs.InstallRaiDrive.HasValue &&
                parsedArgs.InstallRaiDrive.Value)
            {
                steps.Add(new StepItemViewModel
                {
                    Step = _stepFactory.GetStepByName(nameof(OpenWebSiteStep)),
                    Argument = new OpenWebSiteItemViewModel
                    {
                        TargetUrl = CommonStrings.RaiDriveUrl,
                    },
                    TargetSiteName = UIStringResources.Option_Addin,
                    PackageName = UIStringResources.Option_InstallRaiDrive,
                });
            }

            return steps;
        }
    }
}

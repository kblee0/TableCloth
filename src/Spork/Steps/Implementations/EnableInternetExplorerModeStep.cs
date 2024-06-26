﻿using Microsoft.Win32;
using Spork.Components;
using Spork.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using TableCloth.Resources;

namespace Spork.Steps.Implementations
{
    public sealed class EnableInternetExplorerModeStep : StepBase<InstallItemViewModel>
    {
        public EnableInternetExplorerModeStep(
            ICommandLineArguments commandLineArguments)
        {
            _commandLineArguments = commandLineArguments;
        }

        private readonly ICommandLineArguments _commandLineArguments;

        public override Task<bool> EvaluateRequiredStepAsync(InstallItemViewModel _, CancellationToken cancellationToken = default)
        {
            var parsedArgs = _commandLineArguments.GetCurrent();
            var isIeModeRequested = parsedArgs.EnableInternetExplorerMode ?? false;
            return Task.FromResult(!isIeModeRequested);
        }

        public override Task LoadContentForStepAsync(InstallItemViewModel viewModel, Action<double> progressCallback, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public override Task PlayStepAsync(InstallItemViewModel _, Action<double> progressCallback, CancellationToken cancellationToken = default)
        {
            using (var ieModeKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true))
            {
                ieModeKey.SetValue("InternetExplorerIntegrationLevel", 1, RegistryValueKind.DWord);
                ieModeKey.SetValue("InternetExplorerIntegrationSiteList", ConstantStrings.IEModePolicyXmlUrl, RegistryValueKind.String);
            }

            using (var ieModeKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Wow6432Node\Microsoft\Edge", true))
            {
                ieModeKey.SetValue("InternetExplorerIntegrationLevel", 1, RegistryValueKind.DWord);
                ieModeKey.SetValue("InternetExplorerIntegrationSiteList", ConstantStrings.IEModePolicyXmlUrl, RegistryValueKind.String);
            }

            return Task.CompletedTask;
        }

        public override bool ShouldSimulateWhenDryRun
            => true;
    }
}

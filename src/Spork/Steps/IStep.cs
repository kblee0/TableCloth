﻿using Spork.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Spork.Steps
{
    public interface IStep<TInstallItemViewModel> : IStep
        where TInstallItemViewModel : InstallItemViewModel
    {
        Task<bool> EvaluateRequiredStepAsync(TInstallItemViewModel viewModel, CancellationToken cancellationToken = default);

        Task LoadContentForStepAsync(TInstallItemViewModel viewModel, CancellationToken cancellationToken = default);

        Task PlayStepAsync(TInstallItemViewModel viewModel, CancellationToken cancellationToken = default);
    }

    public interface IStep
    {
        Task<bool> EvaluateRequiredStepAsync(InstallItemViewModel viewModel, CancellationToken cancellationToken = default);

        Task LoadContentForStepAsync(InstallItemViewModel viewModel, CancellationToken cancellationToken = default);

        Task PlayStepAsync(InstallItemViewModel viewModel, CancellationToken cancellationToken = default);

        bool ShouldSimulateWhenDryRun { get; }
    }
}

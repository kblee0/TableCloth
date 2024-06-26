﻿using Spork.Components;
using Spork.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TableCloth;
using TableCloth.Resources;

namespace Spork.Steps.Implementations
{
    public sealed class PackageInstallStep : StepBase<PackageInstallItemViewModel>
    {
        public PackageInstallStep(
            ISharedLocations sharedLocations,
            IHttpClientFactory httpClientFactory)
        {
            _sharedLocations = sharedLocations;
            _httpClientFactory = httpClientFactory;
        }

        private ISharedLocations _sharedLocations;
        private IHttpClientFactory _httpClientFactory;

        public override Task<bool> EvaluateRequiredStepAsync(PackageInstallItemViewModel viewModel, CancellationToken cancellationToken = default)
            => Task.FromResult(true);

        public override async Task LoadContentForStepAsync(PackageInstallItemViewModel viewModel, Action<double> progressCallback, CancellationToken cancellationToken = default)
        {
            var downloadFolderPath = _sharedLocations.GetDownloadDirectoryPath();
            var tempFileName = $"installer_{Guid.NewGuid():n}.exe";
            var tempFilePath = System.IO.Path.Combine(downloadFolderPath, tempFileName);

            viewModel.DownloadedFilePath = tempFilePath;

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            var httpClient = _httpClientFactory.CreateGoogleChromeMimickedHttpClient();

            // Note: HttpClient.GetStreamAsync로는 Content-Length 헤더 값을 스트림 길이로 받지 못함.
            using (var response = await httpClient.GetAsync(viewModel.PackageUrl, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var remoteStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using (var fileStream = File.OpenWrite(tempFilePath))
                {
                    await remoteStream.CopyStreamWithProgressAsync(
                        fileStream, new Progress<double>(progressCallback),
                        81920, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public override async Task PlayStepAsync(PackageInstallItemViewModel viewModel, Action<double> progressCallback, CancellationToken cancellationToken = default)
        {
            var tempFilePath = viewModel.DownloadedFilePath;

            var psi = new ProcessStartInfo(tempFilePath, viewModel.Arguments)
            {
                UseShellExecute = false,
            };

            var cpSource = new TaskCompletionSource<int>();
            using (var process = new Process() { StartInfo = psi, })
            {
                process.EnableRaisingEvents = true;
                process.Exited += (_sender, _e) =>
                {
                    var realSender = _sender as Process;
                    cpSource.SetResult(realSender.ExitCode);
                };

                if (!process.Start())
                    TableClothAppException.Throw(ErrorStrings.Error_Package_CanNotStart);

                await cpSource.Task.ConfigureAwait(false);
            }
        }

        public override bool ShouldSimulateWhenDryRun
            => true;
    }
}

﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using TableCloth.Contracts;
using TableCloth.Models.Catalog;
using TableCloth.Models.Configuration;

namespace TableCloth.Models;

public sealed class MainWindowArgumentModel : ITableClothArgumentModel
{
    public MainWindowArgumentModel(
        string[]? selectedServices,
        bool builtFromCommandLine,
        bool? enableMicrophone = default,
        bool? enableWebCam = default,
        bool? enablePrinters = default,
        string? certPrivateKeyPath = default,
        string? certPublicKeyPath = default,
        bool? installEveryonesPrinter = default,
        bool? installAdobeReader = default,
        bool? installHancomOfficeViewer = default,
        bool? installRaiDrive = default,
        bool? enableInternetExplorerMode = default,
        bool showCommandLineHelp = default)
    {
        SelectedServices = selectedServices ?? Enumerable.Empty<string>();
        EnableMicrophone = enableMicrophone;
        EnableWebCam = enableWebCam;
        EnablePrinters = enablePrinters;
        CertPrivateKeyPath = certPrivateKeyPath;
        CertPublicKeyPath = certPublicKeyPath;
        InstallEveryonesPrinter = installEveryonesPrinter;
        InstallAdobeReader = installAdobeReader;
        InstallHancomOfficeViewer = installHancomOfficeViewer;
        InstallRaiDrive = installRaiDrive;
        EnableInternetExplorerMode = enableInternetExplorerMode;
        ShowCommandLineHelp = showCommandLineHelp;
        BuiltFromCommandLine = builtFromCommandLine;
    }

    public bool BuiltFromCommandLine { get; private set; } = false;

    public bool? EnableMicrophone { get; private set; }

    public bool? EnableWebCam { get; private set; }

    public bool? EnablePrinters { get; private set; }

    public string? CertPrivateKeyPath { get; private set; }

    public string? CertPublicKeyPath { get; private set; }

    public bool? InstallEveryonesPrinter { get; private set; }

    public bool? InstallAdobeReader { get; private set; }

    public bool? InstallHancomOfficeViewer { get; private set; }

    public bool? InstallRaiDrive { get; private set; }

    public bool? EnableInternetExplorerMode { get; private set; }

    public bool ShowCommandLineHelp { get; private set; } = false;

    public IEnumerable<string> SelectedServices { get; private set; } = new List<string>();
}
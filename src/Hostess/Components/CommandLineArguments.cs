﻿using System;
using TableCloth.Models;

namespace Hostess.Components
{
    public sealed class CommandLineArguments
    {
        private readonly Lazy<CommandLineArgumentModel> _argvModelFactory
            = new Lazy<CommandLineArgumentModel>(() => CommandLineArgumentModel.ParseFromArgv());

        public CommandLineArgumentModel Current
            => _argvModelFactory.Value;
    }
}

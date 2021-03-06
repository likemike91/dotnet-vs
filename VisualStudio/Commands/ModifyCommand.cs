﻿using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using vswhere;

namespace VisualStudio
{
    class ModifyCommand : Command<ModifyCommandDescriptor>
    {
        readonly WhereService whereService;
        readonly InstallerService installerService;

        public ModifyCommand(ModifyCommandDescriptor descriptor, WhereService whereService, InstallerService installerService)
            : base(descriptor)
        {
            this.whereService = whereService;
            this.installerService = installerService;
        }

        public override async Task ExecuteAsync(TextWriter output)
        {
            var instances = await whereService.GetAllInstancesAsync(Descriptor.Options);

            var instance = new Chooser().Choose(instances, output);

            if (instance != null)
            {
                var args = new List<string>();

                if (Descriptor.WorkloadsAdded.Any() || Descriptor.WorkloadsRemoved.Any() || Descriptor.ExtraArguments.Contains("--config"))
                    args.Add("--passive"); // otherwise let the user to select the workload in the UI

                args.AddRange(Descriptor.WorkloadsAdded);
                args.AddRange(Descriptor.WorkloadsRemoved);

                args.Add("--installPath");
                args.Add(instance.InstallationPath);

                args.AddRange(Descriptor.ExtraArguments);

                await installerService.ModifyAsync(instance.GetChannel(), instance.GetSku(), args, output);
            }
        }
    }
}

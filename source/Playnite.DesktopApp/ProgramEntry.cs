﻿using CommandLine;
using Playnite.Common;
using Playnite.SDK;
using Playnite.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Playnite.DesktopApp
{
    public class ProgramEntry
    {
        [STAThread]
        public static void Main(string[] args)
        {
            FileSystem.CreateDirectory(PlaynitePaths.JitProfilesPath);
            ProfileOptimization.SetProfileRoot(PlaynitePaths.JitProfilesPath);
            ProfileOptimization.StartProfile("desktop");

            var cmdLine = new CmdLineOptions();
            var parsed = Parser.Default.ParseArguments<CmdLineOptions>(Environment.GetCommandLineArgs());
            if (parsed is Parsed<CmdLineOptions> options)
            {
                cmdLine = options.Value;
            }

            SplashScreen splash = null;
            var procCount = Process.GetProcesses().Where(a => a.ProcessName.StartsWith("Playnite.")).Count();
            if (cmdLine.Start.IsNullOrEmpty() && !cmdLine.HideSplashScreen && procCount == 1)
            {
                splash = new SplashScreen("SplashScreen.png");
                splash.Show(false);
            }

            PlayniteSettings.ConfigureLogger();
            var app = new DesktopApplication(new App(), splash, cmdLine);
            app.Run();
        }
    }
}

using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using messages = FolderListener.Resources.Messages;
using System.Threading;
using System.IO.Abstractions;
using System.Configuration;

namespace FolderListener
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            const string configSectionName = "folderListenerConfigurationSection";

            if (ConfigurationManager.GetSection(configSectionName) is FolderListenerConfigurationSection folderListenerConfigurations)
            {
                CultureInfo culture = new CultureInfo(folderListenerConfigurations.ApplicationLanguage);

                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                FolderListener folderListener = new FolderListener(folderListenerConfigurations);

                folderListener.FileCreated += OnActivity;
                folderListener.FileMoved += OnActivity;
                folderListener.RuleMatch += OnActivity;
                folderListener.RuleNotMatch += OnActivity;
                folderListener.Error += OnActivity;

                folderListener.Listen();
                while (true) {
                    var userInput = Console.ReadKey();
                    if (IsBreakCondition(userInput))
                        break;
                }
            }
        }

        static bool IsBreakCondition(ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.C ||
                   keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.Pause;
        }

        static void OnActivity(object o, FileListenerArgs args) => Console.WriteLine(args.Message);
    }
}

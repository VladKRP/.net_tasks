using FolderListener.Configurations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;

namespace FolderListener
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            const string configSectionName = "folderListenerConfigurationSection";

            if (ConfigurationManager.GetSection(configSectionName) is FolderListenerConfigurationSection folderListenerConfigurations)
            {
                CultureInfo culture = new CultureInfo(folderListenerConfigurations.ApplicationLanguage);

                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                try
                {
                    FolderListener folderListener = new FolderListener(folderListenerConfigurations);

                    folderListener.ListeningStarted += OnActivity;
                    folderListener.FileCreated += OnActivity;
                    folderListener.FileMoved += OnActivity;
                    folderListener.FileNameChanged += OnActivity;
                    folderListener.RuleMatch += OnActivity;
                    folderListener.RuleNotMatch += OnActivity;
                    folderListener.Error += OnActivity;

                    folderListener.Listen();
                }
                catch(FolderListenerException exc)
                {
                    Console.WriteLine(exc.Message + $"\n{exc.Description}\n{exc.ResolveCases}");
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                
                Console.ReadLine();
            }
        }

        static void OnActivity(object o, FileListenerArgs args) => Console.WriteLine(args.Message);
    }
}

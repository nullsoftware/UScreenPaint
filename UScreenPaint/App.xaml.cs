using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UScreenPaint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Assembly CurrentAssembly { get; } = Assembly.GetEntryAssembly();

        public static Version Version { get; } = CurrentAssembly.GetName().Version;

        internal string[] SupportedLanguages { get; } = new string[] 
            {
                "ru",
                "uk",
            };

        public App()
        {
            
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            SetupLanguage();
        }

        private void SetupLanguage()
        {
            string currentLanguageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            if (SupportedLanguages.Contains(currentLanguageCode))
            {
                ResourceDictionary langDir = Resources.MergedDictionaries.First(t => t.Source.ToString().EndsWith("/Resources.xaml"));
                Resources.MergedDictionaries.Remove(langDir);
                Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri($"Assets/Localization/Resources.{currentLanguageCode}.xaml", UriKind.Relative)
                });
            }
            //else
            //{
            //    CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            //    CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            //    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //}
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string TITLE = "Фильтр квартир";

        [DllImport("user32", CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")] static extern bool SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var other = FindWindow(null, TITLE);
                if (other != IntPtr.Zero)
                {
                    SendMessage(other, 0x3000, IntPtr.Zero, IntPtr.Zero);
                    Shutdown();
                }
            }
            catch (Exception) { }
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            fk.MainWindow.Instance.ni.Dispose();

            base.OnExit(e);
        }
    }
}

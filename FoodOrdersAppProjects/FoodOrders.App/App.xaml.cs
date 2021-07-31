using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace FoodOrders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static CultureInfo CultureInfo { get; } = Thread.CurrentThread.CurrentCulture;

        static XmlLanguage XmlLanguage => XmlLanguage.GetLanguage(CultureInfo.IetfLanguageTag);

        public App()
        {
            SetCulture();
            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void SetCulture()
        {
            Debug.WriteLine($"{nameof(SetCulture)}", nameof(App));

            Thread.CurrentThread.CurrentUICulture = CultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage));

            Trace.WriteLine($"Current culture is set to:{CultureInfo}", GetType().Name);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Debug.WriteLine($"{nameof(OnDispatcherUnhandledException)}:{sender};{e}", GetType().Name);

            e.Dispatcher.Invoke(() =>
            {
                Exception error = e.Exception;
                string errorMessages = error.GetAllMessages(addStackTrace: true);
                Debug.WriteLine(errorMessages, GetType().Name);

#if DEBUG
                errorMessages = error.GetAllMessages(innerExceptionsFirst: true); //show stack reversed
#else

                    errorMessages = error.GetAllMessages(innerExceptionsFirst: true);
#endif
                string title = "Error";

                MessageBox.Show(MainWindow, errorMessages, title, MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            });
        }


        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine($"{nameof(TaskScheduler_UnobservedTaskException)}", GetType().Name);
            e.SetObserved();
        }
    }
}

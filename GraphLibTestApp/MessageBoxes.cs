using System.Windows;

namespace GraphLibTestApp
{
    class MessageBoxes
    {
        public static void ShowErrorMessage(Window window, string message)
        {
            MessageBox.Show(window, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

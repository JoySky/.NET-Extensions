using System.Windows;
using System.Windows.Controls;

public static class WindowExtensions
{
    /// <summary>
    /// Set the initial focus on the given control
    /// </summary>
    /// <param name="window">Childwindow for which te focus must be set</param>
    /// <param name="focus">Control to set the focus on</param>
    public static void SetInitialFocus(this Window window, Control focus)
    {
        RoutedEventHandler fp = null; // set to null to prevent unassigned compiler error
        fp = delegate
        {
            focus.Focus();
            var tb = focus as TextBox;
            if (tb != null)
            {
                tb.SelectAll();
            }
            // unsubscribe after first execute
            window.GotFocus -= fp;
        };

        window.GotFocus += fp;
    }
}

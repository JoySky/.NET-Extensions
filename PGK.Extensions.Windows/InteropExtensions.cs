
using System;
using System.Windows;
using System.Windows.Interop;

public static class InteropExtensions
{
    public static bool? ShowDialog(this Window win, IntPtr handle)
    {
        var helper = new WindowInteropHelper(win) {Owner = handle};
        return win.ShowDialog();
    }
}

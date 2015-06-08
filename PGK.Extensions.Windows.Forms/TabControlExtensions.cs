using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Extension methods for System.Windows.Forms.TabControl
/// </summary>
public static class TabControlExtensions
{
    private static readonly Dictionary<TabControl, object[]> _hiddenHeaders = new Dictionary<TabControl, object[]>();

    /// <summary>
    /// Hide tab page headers
    /// </summary>
    /// <param name="tabControl">Current TabControl</param>
    public static void HideHeaders(this TabControl tabControl)
    {
        _hiddenHeaders.Add(tabControl, new object[] { tabControl.Appearance, tabControl.ItemSize, tabControl.SizeMode });

        tabControl.Appearance = TabAppearance.FlatButtons;
        tabControl.ItemSize = new Size(0, 1);
        tabControl.SizeMode = TabSizeMode.Fixed;
    }

    /// <summary>
    /// Show tab page headers
    /// </summary>
    /// <param name="tabControl">Current TabControl</param>
    public static void ShowHeaders(this TabControl tabControl)
    {
        if (!_hiddenHeaders.ContainsKey(tabControl))
            return;

        var opt = _hiddenHeaders[tabControl];
        _hiddenHeaders.Remove(tabControl);

        tabControl.Appearance = (TabAppearance)opt[0];
        tabControl.ItemSize = (Size)opt[1];
        tabControl.SizeMode = (TabSizeMode)opt[2];
    }

    /// <summary>
    /// Check, tab page headers are visible or hidden
    /// </summary>
    /// <param name="tabControl">Current TabControl</param>
    /// <returns>Returns true if visible</returns>
    public static bool IsHeadersVisible(this TabControl tabControl)
    {
        return !_hiddenHeaders.ContainsKey(tabControl);
    }

    /// <summary>
    /// Set visibility of tab page headers
    /// </summary>
    /// <param name="tabControl">Current TabControl</param>
    /// <param name="visible">Visibility of tab page headers</param>
    public static void SetHeadersVisible(this TabControl tabControl, bool visible)
    {
        if (visible)
            ShowHeaders(tabControl);
        else
            HideHeaders(tabControl);
    }
}

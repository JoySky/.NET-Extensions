using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Extension methods for System.Windows.Forms.TabPage
/// </summary>
public static class TabPageExtensions
{
    private static readonly Dictionary<TabPage, object[]> _hiddenPages = new Dictionary<TabPage, object[]>();

    /// <summary>
    /// Set visibility of current TabPage in the parent TabControl.TabPages collection
    /// </summary>
    /// <param name="tabPage">Current TabPage</param>
    /// <param name="visible">Visibility of this tab page</param>
    public static void SetVisible(this TabPage tabPage, bool visible)
    {
        if (visible)
            tabPage.ShowTabPage();
        else
            tabPage.HideTabPage();
    }

    /// <summary>
    /// Checks, tab page into the TabControl.TabPages collection or not
    /// </summary>
    /// <param name="tabPage">Current TabPage</param>
    /// <returns>Returns true if visible</returns>
    public static bool IsVisible(this TabPage tabPage)
    {
        var tabControl = tabPage.Parent as TabControl;
        return tabControl != null && tabControl.TabPages.Contains(tabPage);
    }

    /// <summary>
    /// Show tab page.
    /// <para>Returns back previously temporarily deleted tab page to the parent TabControl.TabPages collection</para>
    /// </summary>
    /// <param name="tabPage">Current TabPage</param>
    public static void ShowTabPage(this TabPage tabPage)
    {
        if (!_hiddenPages.ContainsKey(tabPage))
            return;

        var opt = _hiddenPages[tabPage];
        var tabControl = (TabControl)opt[0];
        var index = (int)opt[1];

        _hiddenPages.Remove(tabPage);
        tabControl.TabPages.Insert(index, tabPage);
    }

    /// <summary>
    /// Hide tab page.
    /// <para>Temporarily removes the tab page from the parent TabControl.TabPages collection</para>
    /// </summary>
    /// <param name="tabPage">Current TabPage</param>
    public static void HideTabPage(this TabPage tabPage)
    {
        var tabControl = (TabControl)tabPage.Parent;

        _hiddenPages.Add(tabPage, new object[] { tabControl, tabPage.TabIndex });
        tabControl.TabPages.Remove(tabPage);
    }
}
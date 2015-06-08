using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;

public static class ButtonExtension
{
    /// <summary>
    /// Removes click event from button
    /// </summary>
    /// <param name="btn"></param>
    public static void RemoveClickEvent(this Button btn)
    {
        var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
        if (f1 != null)
        {
            var obj = f1.GetValue(btn);
            var pi = btn.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (EventHandlerList)pi.GetValue(btn, null);
            list.RemoveHandler(obj, list[obj]);
        }
    }
}
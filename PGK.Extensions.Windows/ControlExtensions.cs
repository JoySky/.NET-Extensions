using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

public static class ControlExtension
{
    /// <summary>
    /// Dump Control Templates of WPF Controls
    /// </summary>
    /// <param name="ctrl">Control whose xaml content of ControlTemplate has to fethced</param>
    /// <returns>XAML representation of Control Template of the control</returns>
    public static string DumpControlTemplate(this Control ctrl)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            NewLineOnAttributes = true
        };

        var strbuild = new StringBuilder();
        XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);
        XamlWriter.Save(ctrl.Template, xmlwrite);
        return strbuild.ToString();
    }
}
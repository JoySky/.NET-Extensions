using System;
using System.Windows.Forms;

/// <summary>
/// Extension methods for System.Windows.Forms.ComboBox
/// </summary>
public static class ComboBoxExtensions
{
    /// <summary>
    /// Automatic calculation of the required width of the drop-down list
    /// </summary>
    /// <param name="comboBox">Current ComboBox</param>
    /// <param name="rightSpaceWidth">Additional white space on the right</param>
    /// <param name="minDropDownWidth">Minimum width of the drop-down list. If value is '-1', minimum width == comboBox.Width</param>
    /// <example>
    /// 	<code>
    ///         //Demo #1
    ///         var names = new[] { "Aleksey", "Alexander", "Anton", "Vladislav" };
    ///         comboBox1.DataSource = names; // or comboBox1.Items.AddRange(names);
    ///         comboBox1.Width = 40;
    ///         comboBox1.MeasureDropDownWidth();
    /// 
    ///         //Demo #2
    ///         comboBox1.SizeChanged += (s, e) => comboBox1.MeasureDropDownWidth();
    ///         comboBox1.Width = 150;
    ///         comboBox1.Anchor |= AnchorStyles.Right;
    /// 	</code>
    /// </example>
    /// <remarks>
    /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
    /// </remarks>
    public static void MeasureDropDownWidth(this ComboBox comboBox, int rightSpaceWidth = 15, int minDropDownWidth = -1)
    {
        if(comboBox.Items.Count == 0)
        {
            comboBox.DropDownWidth = Math.Max(comboBox.Width, minDropDownWidth);
            return;
        }

        var graphics = comboBox.CreateGraphics();

        float measureWidth = 0;
        foreach (object item in comboBox.Items)
        {
            string text;
            if (comboBox.DataSource == null || string.IsNullOrEmpty(comboBox.DisplayMember))
            {
                text = item.ToString();
            }
            else
            {
                var propertyInfo = item.GetType().GetProperty(comboBox.DisplayMember);
                text = propertyInfo != null ? propertyInfo.GetValue(item, null).ToString() : item.ToString();
            }

            measureWidth = Math.Max(measureWidth, graphics.MeasureString(text, comboBox.Font).Width);
        }

        var newWidth = (int)Math.Round(measureWidth);
        newWidth += rightSpaceWidth;
        newWidth = Math.Min(newWidth, Screen.GetWorkingArea(comboBox).Width);

        comboBox.DropDownWidth = Math.Max(newWidth, minDropDownWidth == -1 ? comboBox.Width : minDropDownWidth);
        graphics.Dispose();
    }
}
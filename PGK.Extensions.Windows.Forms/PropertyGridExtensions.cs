using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// Extension methods for the PropertyGrid data type
/// </summary>
public static class PropertyGridExtensions
{
    /// <summary>
    /// Moves splitter between two columns
    /// </summary>
    /// <param name="propertyGrid">Source PropertyGrid control</param>
    /// <param name="pos">Splitter position</param>
    /// <example>
    /// 	<code>
    ///         propertyGrid1.MoveSplitterTo(Convert.ToInt32(propertyGrid1.Width * 0.8));
    ///         //Column1 width = 80%, Column2 width = 20%
    /// 	</code>
    /// </example>
    /// <remarks>
    /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
    /// </remarks>
    public static void MoveSplitterTo(this PropertyGrid propertyGrid, int pos)
    {
        FieldInfo fiGridView = propertyGrid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance);
        object oGridView = fiGridView.GetValue(propertyGrid);
        MethodInfo miMoveSplitterTo = oGridView.GetType().GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
        miMoveSplitterTo.Invoke(oGridView, new object[] { pos });
    }
}
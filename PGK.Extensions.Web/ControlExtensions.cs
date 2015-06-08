using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

/// <summary>
///   Extensions for ASP.NET Controls
/// </summary>
public static class ControlExtensions {

    
    /// <summary>
    /// Tests if the array is empty.
    /// </summary>
    /// <param name="array">The array to test.</param>
    /// <returns>True if the array is empty.</returns>
    public static bool IsEmpty(this Array array)
    {
        array.ExceptionIfNullOrEmpty(
            "The array cannot be null.",
            "array");

        return array.Length == 0;
    }

    /// <summary>
    ///   Returns whether the sequence contains a certain amount of elements.
    /// </summary>
    /// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
    /// <param name = "source">The source for this extension method.</param>
    /// <param name = "count">The amount of elements the sequence should contain.</param>
    /// <returns>True when the sequence contains the specified amount of elements, false otherwise.</returns>
    public static bool HasCountOf<T>(this IEnumerable<T> source, int count)
    {
        return source.Take(count + 1).Count() == count;
    }


    #region Find Controls

    /// <summary>
    ///   Performs a typed search of a control within the current naming container.
    /// </summary>
    /// <typeparam name = "T">The control type</typeparam>
    /// <param name = "control">The parent control / naming container to search within.</param>
    /// <param name = "id">The id of the control to be found.</param>
    /// <returns>The found control</returns>
    public static T FindControl<T>(this Control control, string id) where T : class {
        return (control.FindControl(id) as T);
    }

    /// <summary>
    ///   Finds the control.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "control">The root control.</param>
    /// <param name = "comparison">The comparison.</param>
    /// <returns>The T</returns>
    public static T FindControl<T>(this Control control, Func<T, bool> comparison) where T : class {
        foreach (Control child in control.Controls) {
            var ctl = (child as T);
            if ((ctl != null) && comparison(ctl)) return ctl;

            ctl = FindControl(child, comparison);
            if (ctl != null) return ctl;
        }
        return null;
    }

    /// <summary>
    ///   Finds a control by its ID recursively
    /// </summary>
    /// <param name = "control">The root parent control.</param>
    /// <param name = "id">The id of the control to be found.</param>
    /// <returns>The found control</returns>
    public static Control FindControlRecursive(this Control control, string id) {
        foreach (Control child in control.Controls) {
            if ((child.ID != null) && string.Equals(child.ID, id, StringComparison.InvariantCultureIgnoreCase)) return child;

            var ctl = FindControlRecursive(child, id);
            if (ctl != null) return ctl;
        }
        return null;
    }

    /// <summary>
    ///   Finds a control by its ID recursively
    /// </summary>
    /// <typeparam name = "T">The control type</typeparam>
    /// <param name = "control">The root parent control.</param>
    /// <param name = "id">The id of the control to be found.</param>
    /// <returns>The found control</returns>
    public static T FindControlRecursive<T>(this Control control, string id) where T : class {
        foreach (Control child in control.Controls) {
            if ((child.ID != null) && string.Equals(child.ID, id, StringComparison.InvariantCultureIgnoreCase) && (child is T)) return (child as T);

            var ctl = FindControlRecursive<T>(child, id);
            if (ctl != null) return ctl;
        }
        return null;
    }

    /// <summary>
    ///   Returns the first matching parent control.
    /// </summary>
    /// <typeparam name = "T">The typ of the requested parent control.</typeparam>
    /// <param name = "control">The control to start the search on.</param>
    /// <returns>The found control</returns>
    public static T GetParent<T>(this Control control) where T : class {
        var parent = control.Parent;
        while (parent != null) {
            if (parent is T) return (parent as T);
            parent = parent.Parent;
        }
        return null;
    }

    /// <summary>
    ///   Returns all direct child controls matching to the supplied type.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "control">The control.</param>
    /// <returns></returns>
    /// <example>
    ///   <code>
    ///     foreach(var textControl in this.GetChildControlsByType&lt;ITextControl&gt;()) {
    ///     textControl.Text = "...";
    ///     }
    ///   </code>
    /// </example>
    public static IEnumerable<T> GetChildControlsByType<T>(this Control control) where T : class {
        foreach (Control childControl in control.Controls) {
            if (childControl is T) yield return (childControl as T);
        }
    }

    #endregion

    #region Visiblity

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "controls">The controls to be set visible.</param>
    public static void SetVisibility(this Control control, params Control[] controls) {
        control.SetVisibility(true, controls);
    }

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "visible">if set to <c>true</c> [visible].</param>
    /// <param name = "controls">The controls to be set visible.</param>
    public static void SetVisibility(this Control control, bool visible, params Control[] controls) {
        Array.ForEach(controls, c => c.Visible = visible);
    }

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "controlIDs">The control IDs.</param>
    public static void SetVisibility(this Control control, params string[] controlIDs) {
        control.SetVisibility(true, controlIDs);
    }

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "visible">if set to <c>true</c> [visible].</param>
    /// <param name = "controlIDs">The control IDs.</param>
    public static void SetVisibility(this Control control, bool visible, params string[] controlIDs) {
        foreach (var id in controlIDs) {
            var ctl = control.FindControlRecursive(id);
            if (ctl != null)
                ctl.Visible = visible;
        }
    }

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "condition">The condition.</param>
    /// <param name = "controls">The controls to be set visible.</param>
    public static void SetVisibility(this Control control, Predicate<Control> condition, params Control[] controls) {
        Array.ForEach(controls, c => c.Visible = condition(c));
    }

    /// <summary>
    ///   Sets the visibility of one or more controls
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "condition">The condition.</param>
    /// <param name = "controlIDs">The control IDs.</param>
    public static void SetVisibility(this Control control, Predicate<Control> condition, params string[] controlIDs) {
        foreach (var id in controlIDs) {
            var ctl = control.FindControlRecursive(id);
            if (ctl != null) ctl.Visible = condition(ctl);
        }
    }

    /// <summary>
    ///   Switches the visibility of two controls.
    /// </summary>
    /// <param name = "control">The root control.</param>
    /// <param name = "visible">The visible control.</param>
    /// <param name = "notVisible">The not visible control.</param>
    public static void SwitchVisibility(this Control control, Control visible, Control notVisible) {
        visible.Visible = true;
        notVisible.Visible = false;
    }

    #endregion

    /// <summary>
    /// Runs action delegate for all controls and subcontrols in ControlCollection.
    /// </summary>
    /// <param name="controlCollection">The control collection.</param>
    /// <param name="action">The action.</param>
    /// <remarks></remarks>
    public static void ForEachControl(this ControlCollection controlCollection, Action<Control> action)
    {
        if (controlCollection == null)
            return;
        foreach (Control c in controlCollection)
        {
            action(c);
            if (c.HasControls())
            {
                ForEachControl(c.Controls, action);
            }
        }
    }
}
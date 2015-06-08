using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
///   Extension methods for System.Windows.Forms.Control.
/// </summary>
public static class ControlExtensions {
    /// <summary>
    ///   Returns <c>true</c> if target control is in design mode or one of the target's parent is in design mode.
    ///   Othervise returns <c>false</c>.
    /// </summary>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <example>
    ///   bool designMode = this.button1.IsInWinDesignMode();
    /// </example>
    /// <remarks>
    ///   The design mode is set only to direct controls in desgined control/form.
    ///   However the child controls in controls/usercontrols does not flag for "my parent is in design mode".
    ///   The solution is traversion upon parents of target control.
    /// 
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    public static bool IsInWinDesignMode(this Control target) {
        var ret = false;

        var ctl = target;
        while (false == object.ReferenceEquals(ctl, null)) {
            var site = ctl.Site;
            if (false == object.ReferenceEquals(site, null)) {
                if (site.DesignMode) {
                    ret = true;
                    break;
                }
            }
            ctl = ctl.Parent;
        }

        return ret;
    }

    /// <summary>
    ///   Returns <c>true</c> if target control is NOT in design mode and none of the target's parent is NOT in design mode.
    ///   Othervise returns <c>false</c>.
    /// </summary>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <example>
    ///   bool runtimeMode = this.button1.IsInWinRuntimenMode();
    /// </example>
    /// <remarks>
    ///   The design mode is set only to direct controls in desgined control/form.
    ///   However the child controls in controls/usercontrols does not flag for "my parent is in design mode".
    ///   The solution is traversion upon parents of target control.
    /// 
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    public static bool IsInWinRuntimeMode(this Control target) {
        var ret = true;

        var ctl = target;
        while (false == object.ReferenceEquals(ctl, null)) {
            var site = ctl.Site;
            if (false == object.ReferenceEquals(site, null)) {
                if (site.DesignMode) {
                    ret = false;
                    break;
                }
            }
            ctl = ctl.Parent;
        }

        return ret;
    }

    /// <summary>
    ///   Invoke action on UI thread of target control.
    ///   If current thread is other than the UI thread of control, the Control.Invoke will be used.
    ///   Othervise the action is invoked on current thread.
    /// </summary>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <param name = "action">Action to invoke. Can not be null.</param>
    /// <example>
    ///   this.button1.RunInUIThread( ()=> this.button1.Text = "Click me!" );
    /// </example>
    /// <remarks>
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    public static void RunInUIThread(this Control target, Action action) {
        if (target.InvokeRequired) {
            target.Invoke(action);
        }
        else {
            action();
        }
    }

    /// <summary>
    ///   Find parent controls of target control which are (inherits or implements) specified type.
    /// </summary>
    /// <typeparam name = "T">Type of searched controls.</typeparam>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <returns>Return enumerable of parent controls.</returns>
    /// <example>
    ///   var parentPanels = this.button1.FindParentsOfType&lt;Panel&gt;();
    /// 
    ///   var firstParentPanel = this.button1.FindParentsOfType&lt;Panel&gt;().FirstOrDefault();
    /// </example>
    /// <remarks>
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    public static IEnumerable<T> FindParentsOfType<T>(this Control target) where T : class {
        var ctl = target.Parent;
        while (false == object.ReferenceEquals(ctl, null)) {
            var typedControl = ctl as T;
            if (false == object.ReferenceEquals(typedControl, null)) {
                yield return typedControl;
            }

            ctl = ctl.Parent;
        }
    }

    /// <summary>
    ///   Find child controls of target control which are (inherits or implements) specified type.
    /// 
    ///   Overload for: FindChildsOfType&lt;T&gt;(target, false);
    /// </summary>
    /// <typeparam name = "T">Type of searched controls.</typeparam>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <remarks>
    ///   Depth-first search is used.
    /// 
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    /// <returns>Enumerable object with child controls of specified type.</returns>
    public static IEnumerable<T> FindChildsOfType<T>(this Control target) where T : class {
        var result = FindChildsOfType<T>(target, false);
        return result;
    }

    /// <summary>
    ///   Find child controls of target control which are (inherits or implements) specified type.
    /// </summary>
    /// <typeparam name = "T">Type of searched controls.</typeparam>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <param name = "searchChildrenInReturnedControls">If true, the search algorithm will be continue in returned controls. Othervise the returned control will not be searched.</param>
    /// <remarks>
    ///   Depth-first search is used.
    /// 
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    /// <returns>Enumerable object with child controls of specified type.</returns>
    public static IEnumerable<T> FindChildsOfType<T>(this Control target, bool searchChildrenInReturnedControls) where T : class {
        foreach (Control child in target.Controls) {
            var typedControl = child as T;
            if (false == object.ReferenceEquals(typedControl, null)) {
                yield return typedControl;
            }

            if (child.HasChildren) {
                var subChilds = FindChildsOfType<T>(child);
                foreach (var subChild in subChilds) {
                    yield return subChild;
                }
            }
        }
    }
}
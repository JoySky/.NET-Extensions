using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

public static class CanvasExtension
{
    /// <summary>
    /// Adds child control to canvas
    /// </summary>
    /// <typeparam name="T">Child Type to be added</typeparam>
    /// <param name="canvas">Parent canvas</param>
    /// <param name="control">Element to be added</param>
    public static void AddChild<T>(this Canvas canvas, T control)
    {
        var uiElement = control as UIElement;
        if (uiElement != null && !canvas.Children.Contains(uiElement))
            canvas.Children.Add(uiElement);
    }

    /// <summary>
    /// Removes child control from canvas
    /// </summary>
    /// <typeparam name="T">Child Type to be removed</typeparam>
    /// <param name="canvas">Parent canvas</param>
    /// <param name="control">Child to be removed</param>
    public static void RemoveChild<T>(this Canvas canvas, T control)
    {
        var uiElement = control as UIElement;
        if (uiElement != null && canvas.Children.Contains(uiElement))
            canvas.Children.Remove(uiElement);
    }

    /// <summary>
    /// Inserts child control from canvas
    /// </summary>
    /// <typeparam name="T">Child Type to be inserted</typeparam>
    /// <param name="canvas">Parent canvas</param>
    /// <param name="index">Index of child to be added</param>
    /// <param name="control">Child to be inserted</param>
    public static void InsertChild<T>(this Canvas canvas, int index, T control)
    {
        var uiElement = control as UIElement;
        if (uiElement != null && !canvas.Children.Contains(uiElement))
            canvas.Children.Insert(index, uiElement);
    }

    /// <summary>
    /// Gets Canvas Left Position
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <returns></returns>
    public static double GetCanvasLeft<T>(T control)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");
        return (double)uiElement.GetValue(Canvas.LeftProperty);
    }

    /// <summary>
    /// Get Canvas Top
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <returns></returns>
    public static double GetCanvasTop<T>(T control)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");
        return (double)uiElement.GetValue(Canvas.TopProperty);
    }

    /// <summary>
    /// Gets Canvas position
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <returns></returns>
    public static Point GetCanvasPosition<T>(T control)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");

        return new Point(
            (double)uiElement.GetValue(Canvas.LeftProperty),
            (double)uiElement.GetValue(Canvas.TopProperty));
    }

    /// <summary>
    /// Set Canvas left position
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <param name="length"></param>
    public static void SetCanvasLeft<T>(T control, double length)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");
        uiElement.SetValue(Canvas.LeftProperty, length);
    }

    //Get Canvas Top position

    public static void SetCanvasTop<T>(T control, double length)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");
        uiElement.SetValue(Canvas.TopProperty, length);
    }

    //Set Canvas Position

    public static void SetCanvasPosition<T>(T control, Point value)
    {
        var uiElement = control as UIElement;
        if (uiElement == null)
            throw new ArgumentNullException("control");
        uiElement.SetValue(Canvas.LeftProperty, value.X);
        uiElement.SetValue(Canvas.TopProperty, value.Y);
    }
}

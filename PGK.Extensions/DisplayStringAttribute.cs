using System;

/// <summary>
/// Specifies description for a member of the enum type for display to the UI
/// </summary>
/// <see cref="EnumExtensions.DisplayString"/>
/// <example>
///     <code>
///         enum OperatingSystem
///         {
///            [DisplayString("MS-DOS")]
///            Msdos,
///         
///            [DisplayString("Windows 98")]
///            Win98,
///         
///            [DisplayString("Windows XP")]
///            Xp,
///         
///            [DisplayString("Windows Vista")]
///            Vista,
///         
///            [DisplayString("Windows 7")]
///            Seven,
///         }
///         
///         public string GetMyOSName()
///         {
///             var myOS = OperatingSystem.Seven;
///             return myOS.DisplayString();
///         }
///     </code>
/// </example>
/// <remarks>
/// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class DisplayStringAttribute : Attribute
{
    /// <summary>
    /// The default value for the attribute <c>DisplayStringAttribute</c>, which is an empty string
    /// </summary>
    public static readonly DisplayStringAttribute Default = new DisplayStringAttribute();

    private readonly string _displayString;
    /// <summary>
    /// The value of this attribute
    /// </summary>
    public string DisplayString
    {
        get { return _displayString; }
    }

    /// <summary>
    /// Initializes a new instance of the class <c>DisplayStringAttribute</c> with default value (empty string)
    /// </summary>
    public DisplayStringAttribute()
        :this(string.Empty) { }

    /// <summary>
    /// Initializes a new instance of the class <c>DisplayStringAttribute</c> with specified value
    /// </summary>
    /// <param name="displayString">The value of this attribute</param>
    public DisplayStringAttribute(string displayString)
    {
        _displayString = displayString;
    }

    public override bool Equals(object obj)
    {
        var dsaObj = obj as DisplayStringAttribute;
        if (dsaObj == null)
            return false;

        return _displayString.Equals(dsaObj._displayString);
    }

    public override int GetHashCode()
    {
        return _displayString.GetHashCode();
    }

    public override bool IsDefaultAttribute()
    {
        return Equals(Default);
    }
}
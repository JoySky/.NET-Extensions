using System.Windows.Controls;
using System.Windows.Input;

public static class TextboxExtension
{
    #region TextBox

    public static void SetInputScope(this TextBox tb, InputScopeNameValue inputScopeNameValue)
    {
        tb.InputScope = new InputScope
        {
            Names = { new InputScopeName { NameValue = inputScopeNameValue } }
        };
    }

    #endregion
}


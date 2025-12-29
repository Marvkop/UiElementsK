using System.Windows.Input;
using UiElementsK.CodeGen.Attributes;

namespace UiElementsK.Windows;

[DependencyProperty("CloseCommand", typeof(ICommand))]
[DependencyProperty("Test", typeof(int))]
internal partial class CommandWindow : Window
{
}
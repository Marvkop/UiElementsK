using System.Windows.Input;
using UiElementsK.CodeGen.Attributes;

namespace UiElementsK.Windows;

[DependencyProperty("CloseCommand1", typeof(ICommand), false)]
[DependencyProperty("CloseCommand2", typeof(ICommand))]
[DependencyProperty("Test", typeof(int), true)]
internal partial class CommandWindow : Window;
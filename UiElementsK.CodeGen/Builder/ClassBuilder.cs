using System.Text;

namespace UiElementsK.CodeGen.Builder;

internal class ClassBuilder
{
    private readonly StringBuilder _builder = new();
    private readonly string _className;

    private ClassBuilder(string className)
    {
        _className = className;
    }

    public static ClassBuilder CreateFromType(ISymbol symbol)
    {
        var builder = new ClassBuilder(symbol.Name);
        builder._builder.AppendLine(
            $$"""
              namespace {{symbol.ContainingNamespace.ToDisplayString()}};

              {{symbol.DeclaredAccessibility.ToString().ToLower()}} partial class {{symbol.Name}}
              {
              """);

        return builder;
    }

    public void AddDependencyProperty(string name, ITypeSymbol type)
    {
        var typeName = type.ToDisplayString();

        _builder.AppendLine();
        _builder.AppendLine($$"""
                                  public static readonly DependencyProperty {{name}}Property = DependencyProperty.Register(nameof({{name}}), typeof({{typeName}}), typeof({{_className}}), new PropertyMetadata(On{{name}}Changed));
                                  
                                  public {{typeName}} {{name}} {
                                      get => ({{typeName}})GetValue({{name}}Property);
                                      set => SetValue({{name}}Property, value);
                                  }
                                  
                                  private static void On{{name}}Changed(DependencyObject dpo, DependencyPropertyChangedEventArgs args) {
                                      var target = ({{_className}})dpo;
                                      var newValue = ({{typeName}})args.NewValue;
                                      var oldValue = ({{typeName}})args.OldValue;
                                      
                                      target.On{{name}}Changed(newValue, oldValue);
                                  }
                                  
                                  partial void On{{name}}Changed({{typeName}} newValue, {{typeName}} oldValue);
                              """);
        _builder.AppendLine();
    }

    public override string ToString()
    {
        _builder.AppendLine("}");
        return _builder.ToString();
    }
}
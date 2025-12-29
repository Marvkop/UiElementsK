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
        return new ClassBuilder(symbol.Name)
            .AddComment($"generated from: {symbol.ToDisplayString()}")
            .SetNameSpace(symbol.ContainingNamespace.ToDisplayString())
            .SetClass(symbol.DeclaredAccessibility, symbol.Name);
    }

    private ClassBuilder SetNameSpace(string @namespace)
    {
        _builder.AppendLine($"namespace {@namespace};");
        return this;
    }

    private ClassBuilder SetClass(Accessibility accessibility, string name)
    {
        _builder.AppendLine(
            $$"""
            
              {{accessibility.ToString().ToLower()}} partial class {{name}}
              {
              """);

        return this;
    }

    public ClassBuilder AddDependencyProperty(string name, ITypeSymbol type, bool withChangedHandler)
    {
        var typeName = type.ToDisplayString();

        _builder.AppendLine();
        _builder.AppendLine(
            withChangedHandler
                ? $$"""
                        public static readonly DependencyProperty {{name}}Property = DependencyProperty.Register(nameof({{name}}), typeof({{typeName}}), typeof({{_className}}), new PropertyMetadata(On{{name}}Changed));
                        
                        public {{typeName}} {{name}}
                        {
                            get => ({{typeName}})GetValue({{name}}Property);
                            set => SetValue({{name}}Property, value);
                        }
                        
                        private static void On{{name}}Changed(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
                        {
                            var target = ({{_className}})dpo;
                            var newValue = ({{typeName}})args.NewValue;
                            var oldValue = ({{typeName}})args.OldValue;
                            
                            target.On{{name}}Changed(newValue, oldValue);
                        }
                        
                        partial void On{{name}}Changed({{typeName}} newValue, {{typeName}} oldValue);
                    """
                // without changed handler
                : $$"""
                        public static readonly DependencyProperty {{name}}Property = DependencyProperty.Register(nameof({{name}}), typeof({{typeName}}), typeof({{_className}}));
                        
                        public {{typeName}} {{name}}
                        {
                            get => ({{typeName}})GetValue({{name}}Property);
                            set => SetValue({{name}}Property, value);
                        }
                    """);
        _builder.AppendLine();

        return this;
    }

    public ClassBuilder AddComment(string comment, int indent = 0)
    {
        var indentString = new string(' ', indent * 4);

        _builder.AppendLine($"{indentString}/*");
        _builder.AppendLine($"{indentString}{comment}");
        _builder.AppendLine($"{indentString}*/");

        return this;
    }

    public override string ToString()
    {
        _builder.AppendLine("}");

        return _builder.ToString();
    }
}
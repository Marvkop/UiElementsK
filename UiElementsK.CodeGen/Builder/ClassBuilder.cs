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

              public partial class {{symbol.Name}}
              {
              """);

        return builder;
    }

    public void AddDependencyProperty(string name, ITypeSymbol type)
    {
        var typeName = type.ToDisplayString();

        _builder.AppendLine();
        _builder.AppendLine($"    public static readonly DependencyProperty {name}Property = DependencyProperty.Register(nameof({name}), typeof({typeName}), typeof({_className}));");
        _builder.AppendLine();
        _builder.AppendLine($"    public {typeName} {name} {{");
        _builder.AppendLine($"        get => ({typeName})GetValue({name}Property);");
        _builder.AppendLine($"        set => SetValue({name}Property, value);");
        _builder.AppendLine("    }");
        _builder.AppendLine();
    }

    public override string ToString()
    {
        _builder.AppendLine("}");
        return _builder.ToString();
    }
}
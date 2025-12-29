using Microsoft.CodeAnalysis.CSharp.Syntax;
using UiElementsK.CodeGen.Attributes;
using UiElementsK.CodeGen.Builder;

namespace UiElementsK.CodeGen.Generators;

[Generator]
public class DependencyPropertyGenerator : AttributeGenerator<DependencyPropertyAttribute, ClassDeclarationSyntax>
{
    protected override void Execute(SourceProductionContext context, Data data)
    {
        var builder = ClassBuilder.CreateFromType(data.Symbol);

        foreach (var attribute in data.Attributes)
        {
            if (attribute.ConstructorArguments[0] is { Value: string name } &&
                attribute.ConstructorArguments[1] is { Value: ITypeSymbol type })
            {
                var withChangedHandler = attribute.ConstructorArguments.Length is 3 && attribute.ConstructorArguments[2] is { Value: true };

                builder.AddComment($"generated from: {attribute}", 1);
                builder.AddDependencyProperty(name, type, withChangedHandler);
            }
        }

        context.AddSource($"{data.Symbol.Name}.g.cs", builder.ToString());
    }
}
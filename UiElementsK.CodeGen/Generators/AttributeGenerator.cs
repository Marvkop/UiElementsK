using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UiElementsK.CodeGen.Generators;

public abstract class AttributeGenerator<TAttribute, TSyntax> : IIncrementalGenerator
    where TAttribute : Attribute
    where TSyntax : MemberDeclarationSyntax
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.ForAttributeWithMetadataName(typeof(TAttribute).FullName!, IsRelevant, Transform);

        context.RegisterSourceOutput(syntaxProvider, Execute);
    }

    protected abstract void Execute(SourceProductionContext context, Data data);

    private static Data Transform(GeneratorAttributeSyntaxContext context, CancellationToken token) => new((TSyntax)context.TargetNode, context.TargetSymbol, context.Attributes);

    private static bool IsRelevant(SyntaxNode node, CancellationToken token) => node is TSyntax;

    protected record Data(TSyntax Syntax, ISymbol Symbol, ImmutableArray<AttributeData> Attributes);
}
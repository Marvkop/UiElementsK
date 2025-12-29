using System;

namespace UiElementsK.CodeGen.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyPropertyAttribute(string name, Type type, bool withChangedHandler = false) : Attribute
{
    public string Name { get; } = name;

    public Type Type { get; } = type;

    public bool WithChangedHandler { get; } = withChangedHandler;
}
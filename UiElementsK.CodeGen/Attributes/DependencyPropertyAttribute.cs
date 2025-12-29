using System;

namespace UiElementsK.CodeGen.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyPropertyAttribute(string name, Type type) : Attribute
{
    public string Name { get; } = name;

    public Type Type { get; } = type;
}
#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST
{
    public sealed class ParameterNode : Node
    {
        public TypeNode Type { get; }
        public string Name { get; }
        public Expression? DefaultValue { get; }

        public bool HasDefaultValue => DefaultValue != null;

        public ParameterNode(TypeNode type, string name, Expression? defaultValue, TextRange range) : base(range)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            DefaultValue = defaultValue;
        }

        public ParameterNode(TypeNode type, string name, Expression? defaultValue, TextIndex from, TextIndex to) : base(from, to)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            DefaultValue = defaultValue;
        }

        public ParameterNode(TypeNode type, string name, Expression? defaultValue, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            DefaultValue = defaultValue;
        }

        public override string ToString()
        {
            return HasDefaultValue
                ? $"{Type} {Name} = {DefaultValue}"
                : $"{Type} {Name}";
        }
        public override string ToString(Indentation indent) => ToString();
    }
}

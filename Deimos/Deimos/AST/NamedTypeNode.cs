#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public sealed class NamedTypeNode : TypeNode
    {
        public string Name { get; }

        public NamedTypeNode(string name, TextRange range) : base(range)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public NamedTypeNode(string name, TextIndex from, TextIndex to) : base(from, to)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public NamedTypeNode(string name, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public override string ToString() => Name;
    }
}

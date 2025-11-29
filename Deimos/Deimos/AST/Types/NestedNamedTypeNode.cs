#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Types
{
    public sealed class NestedNamedTypeNode : TypeNode
    {
        public TypeNode ParentType { get; }
        public string Name { get; }

        public NestedNamedTypeNode(
            TypeNode parentType,
            string name,
            TextRange range
        ) : base(range)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public NestedNamedTypeNode(
            TypeNode parentType,
            string name,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public NestedNamedTypeNode(
            TypeNode parentType,
            string name,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return $"{ParentType}.{Name}";
        }
    }
}

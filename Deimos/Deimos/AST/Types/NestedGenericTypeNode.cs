#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Types
{
    public sealed class NestedGenericTypeNode : TypeNode
    {
        public TypeNode ParentType { get; }
        public string Name { get; }
        public ReadOnlyArray<TypeNode> TypeArguments { get; }

        public NestedGenericTypeNode(
            TypeNode parentType,
            string name,
            IEnumerable<TypeNode> typeArguments,
            TextRange range
        ) : base(range)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
        }

        public NestedGenericTypeNode(
            TypeNode parentType,
            string name,
            IEnumerable<TypeNode> typeArguments,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
        }

        public NestedGenericTypeNode(
            TypeNode parentType,
            string name,
            IEnumerable<TypeNode> typeArguments,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            ParentType = parentType ?? throw new System.ArgumentNullException(nameof(parentType));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
        }

        public override string ToString()
        {
            var typeArgs = string.Join(", ", TypeArguments);
            return $"{ParentType}.{Name}<{typeArgs}>";
        }
    }
}

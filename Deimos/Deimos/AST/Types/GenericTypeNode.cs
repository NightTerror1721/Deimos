#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Types
{
    public sealed class GenericTypeNode : TypeNode
    {
        public string Name { get; }
        public ReadOnlyArray<TypeNode> TypeArguments { get; }

        public GenericTypeNode(string name, IEnumerable<TypeNode> typeArguments, TextRange range) : base(range)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public GenericTypeNode(string name, IEnumerable<TypeNode> typeArguments, TextIndex from, TextIndex to) : base(from, to)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public GenericTypeNode(string name, IEnumerable<TypeNode> typeArguments, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeArguments = typeArguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public override string ToString()
        {
            var args = string.Join(", ", TypeArguments);
            return $"{Name}<{args}>";
        }
    }
}

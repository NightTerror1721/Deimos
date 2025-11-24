#nullable enable

using Deimos.Lexer;
using System.Collections.Generic;
using System.Linq;

namespace Deimos.AST
{
    public sealed class GenericTypeNode : TypeNode
    {
        public TypeNode BaseType { get; }
        public TypeNode[] TypeArguments { get; }

        public GenericTypeNode(TypeNode baseType, IEnumerable<TypeNode> typeArguments, TextRange range) : base(range)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
            TypeArguments = typeArguments?.ToArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public GenericTypeNode(TypeNode baseType, IEnumerable<TypeNode> typeArguments, TextIndex from, TextIndex to) : base(from, to)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
            TypeArguments = typeArguments?.ToArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public GenericTypeNode(TypeNode baseType, IEnumerable<TypeNode> typeArguments, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
            TypeArguments = typeArguments?.ToArray() ?? throw new System.ArgumentNullException(nameof(typeArguments));
            if (TypeArguments.Length == 0)
                throw new System.ArgumentException("Type arguments cannot be empty.", nameof(typeArguments));
        }

        public override string ToString()
        {
            var args = string.Join(", ", (IEnumerable<TypeNode>)TypeArguments);
            return $"{BaseType}<{args}>";
        }
    }
}

#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Types
{
    public sealed class NullableTypeNode : TypeNode
    {
        public TypeNode BaseType { get; }

        public NullableTypeNode(TypeNode baseType, TextRange range) : base(range)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
        }

        public NullableTypeNode(TypeNode baseType, TextIndex from, TextIndex to) : base(from, to)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
        }

        public NullableTypeNode(TypeNode baseType, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            BaseType = baseType ?? throw new System.ArgumentNullException(nameof(baseType));
        }

        public override string ToString()
        {
            return $"{BaseType}?";
        }
    }
}

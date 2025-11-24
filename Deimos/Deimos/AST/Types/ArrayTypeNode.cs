#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Types
{
    public sealed class ArrayTypeNode : TypeNode
    {
        public TypeNode ElementType { get; }

        public ArrayTypeNode(TypeNode elementType, TextRange range) : base(range)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
        }

        public ArrayTypeNode(TypeNode elementType, TextIndex from, TextIndex to) : base(from, to)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
        }

        public ArrayTypeNode(TypeNode elementType, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
        }

        public override string ToString()
        {
            return $"{ElementType}[]";
        }
    }
}

#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class NewArrayExpression : Expression
    {
        public TypeNode ElementType { get; }
        public Expression Size { get; }

        public NewArrayExpression(TypeNode elementType, Expression size, TextRange range) : base(range)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
            Size = size ?? throw new System.ArgumentNullException(nameof(size));
        }

        public NewArrayExpression(TypeNode elementType, Expression size, TextIndex from, TextIndex to) : base(from, to)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
            Size = size ?? throw new System.ArgumentNullException(nameof(size));
        }

        public NewArrayExpression(TypeNode elementType, Expression size, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            ElementType = elementType ?? throw new System.ArgumentNullException(nameof(elementType));
            Size = size ?? throw new System.ArgumentNullException(nameof(size));
        }

        public override string ToString()
        {
            return $"new {ElementType}[{Size}]";
        }
    }
}

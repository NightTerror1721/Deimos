#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class TypeReferenceExpression : Expression
    {
        public TypeNode Type { get; }

        public TypeReferenceExpression(
            TypeNode type,
            TextRange range
        ) : base(range)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
        }

        public TypeReferenceExpression(
            TypeNode type,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
        }

        public TypeReferenceExpression(
            TypeNode type,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}

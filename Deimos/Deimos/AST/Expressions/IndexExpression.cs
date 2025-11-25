#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class IndexExpression : Expression
    {
        public Expression Target { get; }
        public Expression Index { get; }

        public IndexExpression(Expression target, Expression index, TextRange range) : base(range)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            Index = index ?? throw new System.ArgumentNullException(nameof(index));
        }

        public IndexExpression(Expression target, Expression index, TextIndex from, TextIndex to) : base(from, to)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            Index = index ?? throw new System.ArgumentNullException(nameof(index));
        }

        public IndexExpression(Expression target, Expression index, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            Index = index ?? throw new System.ArgumentNullException(nameof(index));
        }

        public override string ToString()
        {
            return $"{Target}[{Index}]";
        }
    }
}

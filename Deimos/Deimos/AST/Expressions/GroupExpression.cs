#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class GroupExpression : Expression
    {
        public Expression InnerExpression { get; }

        public GroupExpression(Expression innerExpression, TextRange range) : base(range)
        {
            InnerExpression = innerExpression ?? throw new System.ArgumentNullException(nameof(innerExpression));
        }

        public GroupExpression(Expression innerExpression, TextIndex from, TextIndex to) : base(from, to)
        {
            InnerExpression = innerExpression ?? throw new System.ArgumentNullException(nameof(innerExpression));
        }

        public GroupExpression(Expression innerExpression, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            InnerExpression = innerExpression ?? throw new System.ArgumentNullException(nameof(innerExpression));
        }

        public override string ToString()
        {
            return $"({InnerExpression})";
        }
    }
}

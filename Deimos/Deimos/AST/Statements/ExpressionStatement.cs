#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST.Statements
{
    public sealed class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression, TextRange range) : base(range)
        {
            Expression = expression ?? throw new System.ArgumentNullException(nameof(expression));
        }

        public ExpressionStatement(Expression expression, TextIndex from, TextIndex to) : base(from, to)
        {
            Expression = expression ?? throw new System.ArgumentNullException(nameof(expression));
        }

        public ExpressionStatement(Expression expression, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Expression = expression ?? throw new System.ArgumentNullException(nameof(expression));
        }

        public override string ToString(Indentation indent)
        {
            return $"{Expression};";
        }
    }
}

#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class TernaryExpression : Expression
    {
        public Expression Condition { get; }
        public Expression TrueExpression { get; }
        public Expression FalseExpression { get; }

        public TernaryExpression(Expression condition, Expression trueExpression, Expression falseExpression, TextRange range) : base(range)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            TrueExpression = trueExpression ?? throw new System.ArgumentNullException(nameof(trueExpression));
            FalseExpression = falseExpression ?? throw new System.ArgumentNullException(nameof(falseExpression));
        }

        public TernaryExpression(Expression condition, Expression trueExpression, Expression falseExpression, TextIndex from, TextIndex to) : base(from, to)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            TrueExpression = trueExpression ?? throw new System.ArgumentNullException(nameof(trueExpression));
            FalseExpression = falseExpression ?? throw new System.ArgumentNullException(nameof(falseExpression));
        }

        public TernaryExpression(Expression condition, Expression trueExpression, Expression falseExpression, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            TrueExpression = trueExpression ?? throw new System.ArgumentNullException(nameof(trueExpression));
            FalseExpression = falseExpression ?? throw new System.ArgumentNullException(nameof(falseExpression));
        }

        public override string ToString()
        {
            return $"{Condition} ? {TrueExpression} : {FalseExpression}";
        }
    }
}

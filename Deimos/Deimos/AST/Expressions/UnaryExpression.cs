#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryOperator Operator { get; }
        public Expression Operand { get; }

        public bool IsPostfix => Operator.IsPostfix();
        public bool IsPrefix => Operator.IsPrefix();

        public UnaryExpression(UnaryOperator op, Expression operand, TextRange range) : base(range)
        {
            Operator = op;
            Operand = operand;
        }
        public UnaryExpression(UnaryOperator op, Expression operand, TextIndex from, TextIndex to) : base(from, to)
        {
            Operator = op;
            Operand = operand;
        }
        public UnaryExpression(UnaryOperator op, Expression operand, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Operator = op;
            Operand = operand;
        }

        public override string ToString()
        {
            return IsPrefix
                ? $"{Operator.ToSymbol()}{Operand}"
                : $"{Operand}{Operator.ToSymbol()}";
        }
    }

    public enum UnaryOperator
    {
        LogicalNot,         // !  (boolean), (prefix)
        NonNullAssertion,   // !  (null-forgiving), (postfix)
        BinaryNot,          // ~  (bitwise) (prefix)
        Negation,           // -  (arithmetic), (prefix)
        Positivation,       // +  (arithmetic), (prefix)
        PreIncrement,       // ++ (increment), (prefix)
        PreDecrement,       // -- (decrement), (prefix)
        PostIncrement,      // ++ (increment), (postfix)
        PostDecrement,      // -- (decrement), (postfix)
    }

    public static class UnaryOperatorUtils
    {
        public static bool IsPostfix(this UnaryOperator op) =>
            op == UnaryOperator.NonNullAssertion ||
            op == UnaryOperator.PostIncrement ||
            op == UnaryOperator.PostDecrement;

        public static bool IsPrefix(this UnaryOperator op) => !op.IsPostfix();

        public static string ToSymbol(this UnaryOperator op) => op switch
        {
            UnaryOperator.LogicalNot => "!",
            UnaryOperator.NonNullAssertion => "!",
            UnaryOperator.BinaryNot => "~",
            UnaryOperator.Negation => "-",
            UnaryOperator.Positivation => "+",
            UnaryOperator.PreIncrement => "++",
            UnaryOperator.PreDecrement => "--",
            UnaryOperator.PostIncrement => "++",
            UnaryOperator.PostDecrement => "--",
            _ => throw new System.ArgumentOutOfRangeException(nameof(op), $"Unhandled unary operator: {op}"),
        };
    }
}

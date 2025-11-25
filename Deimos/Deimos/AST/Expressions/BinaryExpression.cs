#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public BinaryOperator Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, BinaryOperator op, Expression right, TextRange range) : base(range)
        {
            Left = left ?? throw new System.ArgumentNullException(nameof(left));
            Operator = op;
            Right = right ?? throw new System.ArgumentNullException(nameof(right));
        }

        public BinaryExpression(Expression left, BinaryOperator op, Expression right, TextIndex from, TextIndex to) : base(from, to)
        {
            Left = left ?? throw new System.ArgumentNullException(nameof(left));
            Operator = op;
            Right = right ?? throw new System.ArgumentNullException(nameof(right));
        }

        public BinaryExpression(Expression left, BinaryOperator op, Expression right, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Left = left ?? throw new System.ArgumentNullException(nameof(left));
            Operator = op;
            Right = right ?? throw new System.ArgumentNullException(nameof(right));
        }

        public override string ToString()
        {
            return $"{Left} {Operator.ToSymbol()} {Right}";
        }
    }

    public enum BinaryOperator
    {
        // Arithmetic Operators
        Addition,                   // +
        Subtraction,                // -
        Multiplication,             // *
        Division,                   // /
        Modulus,                    // %
        Exponentiation,             // **

        // Comparison Operators
        Equal,                      // ==
        NotEqual,                   // !=
        LessThan,                   // <
        LessThanOrEqual,            // <=
        GreaterThan,                // >
        GreaterThanOrEqual,         // >=

        // Logical Operators
        LogicalAnd,                 // &&
        LogicalOr,                  // ||
        NullCoalescing,             // ??

        // Bitwise Operators
        BitwiseAnd,                 // &
        BitwiseOr,                  // |
        BitwiseXor,                 // ^
        LeftShift,                  // <<
        RightShift,                 // >>

        // Assignment Operators
        Assignment,                 // =
        AdditionAssignment,         // +=
        SubtractionAssignment,      // -=
        MultiplicationAssignment,   // *=
        DivisionAssignment,         // /=
        ModulusAssignment,          // %=
        ExponentiationAssignment,   // **=
        BitewiseAndAssignment,      // &=
        BitewiseOrAssignment,       // |=
        BitewiseXorAssignment,      // ^=
        LeftShiftAssignment,        // <<=
        RightShiftAssignment,       // >>=
        NullCoalesceAssignment,     // ??=

        // Other Operators
        IdentityCompare,            // is
        Contains,                   // in
    }

    public static class BinaryOperatorUtils
    {
        public static int GetPrecedence(this BinaryOperator op)
        {
            return op switch
            {
                BinaryOperator.Exponentiation => 13,

                BinaryOperator.Multiplication or
                BinaryOperator.Division or
                BinaryOperator.Modulus => 12,

                BinaryOperator.Addition or
                BinaryOperator.Subtraction => 11,

                BinaryOperator.LeftShift or
                BinaryOperator.RightShift => 10,

                BinaryOperator.IdentityCompare => 9,

                BinaryOperator.GreaterThan or
                BinaryOperator.GreaterThanOrEqual or
                BinaryOperator.LessThan or
                BinaryOperator.LessThanOrEqual or
                BinaryOperator.Contains => 8,

                BinaryOperator.Equal or
                BinaryOperator.NotEqual => 7,

                BinaryOperator.BitwiseAnd => 6,

                BinaryOperator.BitwiseXor => 5,

                BinaryOperator.BitwiseOr => 4,

                BinaryOperator.LogicalAnd => 3,

                BinaryOperator.LogicalOr => 2,

                BinaryOperator.NullCoalescing => 2,

                BinaryOperator.Assignment or
                BinaryOperator.AdditionAssignment or
                BinaryOperator.SubtractionAssignment or
                BinaryOperator.MultiplicationAssignment or
                BinaryOperator.DivisionAssignment or
                BinaryOperator.ModulusAssignment or
                BinaryOperator.ExponentiationAssignment or
                BinaryOperator.BitewiseAndAssignment or
                BinaryOperator.BitewiseOrAssignment or
                BinaryOperator.LeftShiftAssignment or
                BinaryOperator.RightShiftAssignment or
                BinaryOperator.NullCoalesceAssignment => 1,

                _ => 0
            };
        }

        public static bool IsRightAssociative(this BinaryOperator op)
        {
            return op switch
            {
                BinaryOperator.Exponentiation => true,
                BinaryOperator.Assignment or
                BinaryOperator.AdditionAssignment or
                BinaryOperator.SubtractionAssignment or
                BinaryOperator.MultiplicationAssignment or
                BinaryOperator.DivisionAssignment or
                BinaryOperator.ModulusAssignment or
                BinaryOperator.ExponentiationAssignment or
                BinaryOperator.BitewiseAndAssignment or
                BinaryOperator.BitewiseOrAssignment or
                BinaryOperator.LeftShiftAssignment or
                BinaryOperator.RightShiftAssignment or
                BinaryOperator.NullCoalesceAssignment => true,
                _ => false,
            };
        }

        public static bool IsLeftAssociative(this BinaryOperator op) => !op.IsRightAssociative();

        public static bool IsAssignmentOperator(this BinaryOperator op)
        {
            return op switch
            {
                BinaryOperator.Assignment or
                BinaryOperator.AdditionAssignment or
                BinaryOperator.SubtractionAssignment or
                BinaryOperator.MultiplicationAssignment or
                BinaryOperator.DivisionAssignment or
                BinaryOperator.ModulusAssignment or
                BinaryOperator.ExponentiationAssignment or
                BinaryOperator.BitewiseAndAssignment or
                BinaryOperator.BitewiseOrAssignment or
                BinaryOperator.LeftShiftAssignment or
                BinaryOperator.RightShiftAssignment or
                BinaryOperator.NullCoalesceAssignment => true,
                _ => false,
            };
        }

        public static string ToSymbol(this BinaryOperator op)
        {
            return op switch
            {
                BinaryOperator.Addition => "+",
                BinaryOperator.Subtraction => "-",
                BinaryOperator.Multiplication => "*",
                BinaryOperator.Division => "/",
                BinaryOperator.Modulus => "%",
                BinaryOperator.Exponentiation => "**",
                BinaryOperator.Equal => "==",
                BinaryOperator.NotEqual => "!=",
                BinaryOperator.LessThan => "<",
                BinaryOperator.LessThanOrEqual => "<=",
                BinaryOperator.GreaterThan => ">",
                BinaryOperator.GreaterThanOrEqual => ">=",
                BinaryOperator.LogicalAnd => "&&",
                BinaryOperator.LogicalOr => "||",
                BinaryOperator.NullCoalescing => "??",
                BinaryOperator.BitwiseAnd => "&",
                BinaryOperator.BitwiseOr => "|",
                BinaryOperator.BitwiseXor => "^",
                BinaryOperator.LeftShift => "<<",
                BinaryOperator.RightShift => ">>",
                BinaryOperator.Assignment => "=",
                BinaryOperator.AdditionAssignment => "+=",
                BinaryOperator.SubtractionAssignment => "-=",
                BinaryOperator.MultiplicationAssignment => "*=",
                BinaryOperator.DivisionAssignment => "/=",
                BinaryOperator.ModulusAssignment => "%=",
                BinaryOperator.ExponentiationAssignment => "**=",
                BinaryOperator.BitewiseAndAssignment => "&=",
                BinaryOperator.BitewiseOrAssignment => "|=",
                BinaryOperator.BitewiseXorAssignment => "^=",
                BinaryOperator.LeftShiftAssignment => "<<=",
                BinaryOperator.RightShiftAssignment => ">>=",
                BinaryOperator.NullCoalesceAssignment => "??=",
                BinaryOperator.IdentityCompare => "is",
                BinaryOperator.Contains => "in",
                _ => throw new System.ArgumentOutOfRangeException(nameof(op), $"Unhandled binary operator: {op}"),
            };
        }
    }
}

#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class InstanceOfExpression : Expression
    {
        public Expression Value { get; }
        public TypeNode TargetType { get; }

        public InstanceOfExpression(Expression value, TypeNode targetType, TextRange range) : base(range)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            TargetType = targetType ?? throw new System.ArgumentNullException(nameof(targetType));
        }

        public InstanceOfExpression(Expression value, TypeNode targetType, TextIndex from, TextIndex to) : base(from, to)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            TargetType = targetType ?? throw new System.ArgumentNullException(nameof(targetType));
        }

        public InstanceOfExpression(Expression value, TypeNode targetType, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            TargetType = targetType ?? throw new System.ArgumentNullException(nameof(targetType));
        }

        public override string ToString()
        {
            return $"{Value} instanceof {TargetType}";
        }
    }
}

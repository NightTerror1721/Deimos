#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST.Statements
{
    public sealed class ReturnStatement : Statement
    {
        public Expression? Value { get; }

        public ReturnStatement(
            Expression? value,
            TextRange range
        ) : base(range)
        {
            Value = value;
        }

        public ReturnStatement(
            Expression? value,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Value = value;
        }

        public ReturnStatement(
            Expression? value,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Value = value;
        }

        public override string ToString(Indentation indent)
        {
            if (Value != null)
                return $"return {Value};";
            else
                return $"return;";
        }
    }
}

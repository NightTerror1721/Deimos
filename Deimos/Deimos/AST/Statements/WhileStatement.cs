#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class WhileStatement : Statement
    {
        public Expression Condition { get; }
        public Statement Body { get; }

        public WhileStatement(
            Expression condition,
            Statement body,
            TextRange range
        ) : base(range)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public WhileStatement(
            Expression condition,
            Statement body,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public WhileStatement(
            Expression condition,
            Statement body,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.Append($"while ({Condition})");
            if (Body is BlockStatement)
                sb.AppendLine(Body.ToString(indent));
            else
            {
                Indentation indentIncreased = indent.Increase();
                sb.AppendLine();
                sb.Append(indentIncreased.ToString()).AppendLine(Body.ToString(indentIncreased));
            }
            return sb.ToString().TrimEnd();
        }
    }
}

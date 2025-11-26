#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class ForStatement : Statement
    {
        public Statement? Initializer { get; }
        public Expression? Condition { get; }
        public Expression? Iterator { get; }
        public Statement Body { get; }

        public ForStatement(
            Statement? initializer,
            Expression? condition,
            Expression? iterator,
            Statement body,
            TextRange range
        ) : base(range)
        {
            Initializer = initializer;
            Condition = condition;
            Iterator = iterator;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public ForStatement(
            Statement? initializer,
            Expression? condition,
            Expression? iterator,
            Statement body,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Initializer = initializer;
            Condition = condition;
            Iterator = iterator;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public ForStatement(
            Statement? initializer,
            Expression? condition,
            Expression? iterator,
            Statement body,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Initializer = initializer;
            Condition = condition;
            Iterator = iterator;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.Append($"for (");

            if (Initializer != null)
                sb.Append(Initializer.ToString().Trim());
            sb.Append("; ");

            if (Condition != null)
                sb.Append(Condition.ToString().Trim());
            sb.Append("; ");

            if (Iterator != null)
                sb.Append(Iterator.ToString().Trim());
            sb.Append(")");

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

#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class IfStatement : Statement
    {
        public Expression Condition { get; }
        public Statement ThenBranch { get; }
        public Statement? ElseBranch { get; }

        public bool HasElseBranch => ElseBranch != null;

        public IfStatement(
            Expression condition,
            Statement thenBranch,
            Statement? elseBranch,
            TextRange range
        ) : base(range)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            ThenBranch = thenBranch ?? throw new System.ArgumentNullException(nameof(thenBranch));
            ElseBranch = elseBranch;
        }

        public IfStatement(
            Expression condition,
            Statement thenBranch,
            Statement? elseBranch,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            ThenBranch = thenBranch ?? throw new System.ArgumentNullException(nameof(thenBranch));
            ElseBranch = elseBranch;
        }

        public IfStatement(
            Expression condition,
            Statement thenBranch,
            Statement? elseBranch,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            ThenBranch = thenBranch ?? throw new System.ArgumentNullException(nameof(thenBranch));
            ElseBranch = elseBranch;
        }

        public override string ToString(Indentation indent)
        {
            var indentStr = indent.ToString();
            var sb = new StringBuilder();

            sb.Append($"if ({Condition}) ");
            if (ThenBranch is BlockStatement)
                sb.AppendLine(ThenBranch.ToString(indent));
            else
            {
                Indentation indentIncreased = indent.Increase();
                sb.AppendLine();
                sb.Append(indentIncreased.ToString()).AppendLine(ThenBranch.ToString(indentIncreased));
            }

            if (ElseBranch != null)
            {
                sb.Append($"{indentStr}else ");
                if (ElseBranch is BlockStatement)
                    sb.AppendLine(ElseBranch.ToString(indent));
                else
                {
                    Indentation indentIncreased = indent.Increase();
                    sb.AppendLine();
                    sb.Append(indentIncreased.ToString()).AppendLine(ElseBranch.ToString(indentIncreased));
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}

#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class ForeachStatement : Statement
    {
        public TypeNode? ElementType { get; }
        public string ElementName { get; }
        public Expression Collection { get; }
        public Statement Body { get; }

        public ForeachStatement(
            TypeNode? elementType,
            string elementName,
            Expression collection,
            Statement body,
            TextRange range
        ) : base(range)
        {
            ElementType = elementType;
            ElementName = elementName ?? throw new System.ArgumentNullException(nameof(elementName));
            Collection = collection ?? throw new System.ArgumentNullException(nameof(collection));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public ForeachStatement(
            TypeNode? elementType,
            string elementName,
            Expression collection,
            Statement body,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            ElementType = elementType;
            ElementName = elementName ?? throw new System.ArgumentNullException(nameof(elementName));
            Collection = collection ?? throw new System.ArgumentNullException(nameof(collection));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public ForeachStatement(
            TypeNode? elementType,
            string elementName,
            Expression collection,
            Statement body,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            ElementType = elementType;
            ElementName = elementName ?? throw new System.ArgumentNullException(nameof(elementName));
            Collection = collection ?? throw new System.ArgumentNullException(nameof(collection));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.Append($"foreach (");
            if (ElementType != null)
                sb.Append($"{ElementType} ");
            sb.Append($"{ElementName} in {Collection})");
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

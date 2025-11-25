#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class BlockStatement : Statement
    {
        public ReadOnlyArray<Statement> Statements { get; }

        public BlockStatement(IEnumerable<Statement> statements, TextRange range) : base(range)
        {
            Statements = statements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(statements));
        }

        public BlockStatement(IEnumerable<Statement> statements, TextIndex from, TextIndex to) : base(from, to)
        {
            Statements = statements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(statements));
        }

        public BlockStatement(IEnumerable<Statement> statements, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Statements = statements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(statements));
        }

        public override string ToString(Indentation indent)
        {
            if (Statements.Count == 0)
                return "{ }";

            Indentation newIndent = indent.Increase();
            var newIndentStr = newIndent.ToString();

            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var statement in Statements)
                sb.Append(newIndentStr).AppendLine(statement.ToString());
            sb.Append(indent).AppendLine("}");
            return sb.ToString();
        }
    }
}

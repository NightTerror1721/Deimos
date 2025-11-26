#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;
using System.Text;

namespace Deimos.AST.Statements
{
    public sealed class SwitchStatement : Statement
    {
        public Expression Value { get; }
        public ReadOnlyArray<SwitchSection> Sections { get; }

        public SwitchStatement(Expression value, IEnumerable<SwitchSection> sections, TextRange range) : base(range)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            Sections = sections?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(sections));
        }

        public SwitchStatement(Expression value, IEnumerable<SwitchSection> sections, TextIndex from, TextIndex to) : base(from, to)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            Sections = sections?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(sections));
        }

        public SwitchStatement(Expression value, IEnumerable<SwitchSection> sections, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            Sections = sections?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(sections));
        }

        public override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"switch ({Value})");
            sb.AppendLine(indent + "{");
            Indentation indentIncreased = indent.Increase();
            foreach (var section in Sections)
                sb.Append(indentIncreased).AppendLine(section.ToString(indentIncreased));
            sb.Append(indent).Append("}");
            return sb.ToString();
        }
    }

    public sealed class SwitchSection : Node
    {
        public SwitchLabel Label { get; }
        public Statement Body { get; }

        public bool IsDefault => Label.IsDefault;

        public SwitchSection(SwitchLabel label, Statement body, TextRange range) : base(range)
        {
            Label = label;
            Body = body;
        }

        public SwitchSection(SwitchLabel label, Statement body, TextIndex from, TextIndex to) : base(from, to)
        {
            Label = label;
            Body = body;
        }

        public SwitchSection(SwitchLabel label, Statement body, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Label = label;
            Body = body;
        }

        public override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Label.ToString(indent));

            if (Body is BlockStatement)
                sb.AppendLine(Body.ToString(indent));
            else
            {
                Indentation indentIncreased = indent.Increase();
                sb.Append(indentIncreased).AppendLine(Body.ToString(indentIncreased));
            }
            return sb.ToString().TrimEnd();
        }
    }

    public sealed class SwitchLabel : Node
    {
        public Expression? LabelExpression { get; }
        public bool IsDefault => LabelExpression == null;

        public SwitchLabel(Expression? labelExpression, TextRange range) : base(range)
        {
            LabelExpression = labelExpression;
        }

        public SwitchLabel(Expression? labelExpression, TextIndex from, TextIndex to) : base(from, to)
        {
            LabelExpression = labelExpression;
        }

        public SwitchLabel(Expression? labelExpression, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            LabelExpression = labelExpression;
        }

        public override string ToString(Indentation indent)
        {
            return IsDefault ? "default:" : $"case {LabelExpression}:";
        }
    }
}

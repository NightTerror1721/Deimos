#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class IdentifierExpression : Expression
    {
        public string Name { get; }

        public IdentifierExpression(string name, TextRange range) : base(range)
        {
            Name = name;
        }

        public IdentifierExpression(string name, TextIndex from, TextIndex to) : base(from, to)
        {
            Name = name;
        }

        public IdentifierExpression(string name, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}

#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public abstract class Expression : Node
    {
        protected Expression(TextRange range) : base(range) { }

        protected Expression(TextIndex from, TextIndex to) : base(from, to) { }

        protected Expression(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }
    }
}

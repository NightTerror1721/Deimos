#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public abstract class Statement : Node
    {
        protected Statement(TextRange range) : base(range) { }

        protected Statement(TextIndex from, TextIndex to) : base(from, to) { }

        protected Statement(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }
    }
}

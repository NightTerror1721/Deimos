#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public abstract class Declaration : Node
    {
        protected Declaration(TextRange range) : base(range) { }

        protected Declaration(TextIndex from, TextIndex to) : base(from, to) { }

        protected Declaration(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }
    }
}

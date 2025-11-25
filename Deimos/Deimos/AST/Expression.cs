#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST
{
    public abstract class Expression : Node
    {
        protected Expression(TextRange range) : base(range) { }

        protected Expression(TextIndex from, TextIndex to) : base(from, to) { }

        protected Expression(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }

        public abstract override string ToString();
        public sealed override string ToString(Indentation indent) => ToString();
    }
}

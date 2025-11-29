#nullable enable

using Deimos.Lexer;

namespace Deimos.Exceptions
{
    public class DeimosParserException : DeimosLocatedTextCompilerException
    {
        public DeimosParserException(string message, TextRange range) : base(message, range) { }

        public DeimosParserException(string message, TextIndex from, TextIndex to) :
            base(message, TextRange.From(from, to))
        { }

        public DeimosParserException(string message, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(message, TextRange.From(fromLine, fromColumn, toLine, toColumn))
        { }

        public DeimosParserException(string message, Token token) : base(message, token.Range) { }
    }
}

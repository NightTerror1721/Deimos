#nullable enable

using Deimos.Lexer;

namespace Deimos.Exceptions
{
    public abstract class DeimosLocatedTextCompilerException : DeimosCompilerException
    {
        public TextRange Range { get; }
        public TextIndex Start => Range.Start;
        public TextIndex End => Range.End;
        public int Line => Start.Line;
        public int Column => Start.Column;

        public DeimosLocatedTextCompilerException(string message, TextRange range) : base($"{message} ({range})")
        {
            Range = range;
        }
    }
}

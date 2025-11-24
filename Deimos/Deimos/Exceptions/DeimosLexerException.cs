#nullable enable

using Deimos.Lexer;

namespace Deimos.Exceptions
{
    public class DeimosLexerException : DeimosLocatedTextCompilerException
    {
        public Token Token { get; }

        public DeimosLexerException(string message, Token token) : base(message, token.Range)
        {
            Token = token ?? throw new System.ArgumentNullException(nameof(token));
        }
    }
}

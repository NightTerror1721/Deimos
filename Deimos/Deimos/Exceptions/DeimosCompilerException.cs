#nullable enable

using Deimos.Lexer;
using System;

namespace Deimos.Exceptions
{
    public class DeimosCompilerException : DeimosException
    {
        public DeimosCompilerException(string message) : base(message) { }
        public DeimosCompilerException(string message, Exception innerException) : base(message, innerException) { }
    }
}

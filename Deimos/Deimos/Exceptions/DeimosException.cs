#nullable enable

using System;

namespace Deimos.Exceptions
{
    public class DeimosException : Exception
    {
        public DeimosException(string message) : base(message) { }
        public DeimosException(string message, Exception innerException) : base(message, innerException) { }
    }
}

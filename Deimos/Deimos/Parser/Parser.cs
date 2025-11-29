#nullable enable

namespace Deimos.Parser
{
    public sealed partial class Parser
    {
        private readonly TokenReader _reader;

        public Parser(TokenReader reader)
        {
            _reader = reader ?? throw new System.ArgumentNullException(nameof(reader));
        }
    }
}

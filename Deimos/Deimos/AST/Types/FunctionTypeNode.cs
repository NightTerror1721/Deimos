#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Types
{
    public sealed class FunctionTypeNode : TypeNode
    {
        public TypeNode ReturnType { get; }
        public ReadOnlyArray<TypeNode> ParameterTypes { get; }

        public FunctionTypeNode(TypeNode returnType, IEnumerable<TypeNode> parameterTypes, TextRange range) : base(range)
        {
            ReturnType = returnType ?? throw new System.ArgumentNullException(nameof(returnType));
            ParameterTypes = parameterTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameterTypes));
        }

        public FunctionTypeNode(TypeNode returnType, IEnumerable<TypeNode> parameterTypes, TextIndex from, TextIndex to) : base(from, to)
        {
            ReturnType = returnType ?? throw new System.ArgumentNullException(nameof(returnType));
            ParameterTypes = parameterTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameterTypes));
        }

        public FunctionTypeNode(TypeNode returnType, IEnumerable<TypeNode> parameterTypes, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            ReturnType = returnType ?? throw new System.ArgumentNullException(nameof(returnType));
            ParameterTypes = parameterTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameterTypes));
        }

        public override string ToString()
        {
            var parameters = string.Join(", ", ParameterTypes);
            return $"({parameters}): {ReturnType}";
        }
    }
}

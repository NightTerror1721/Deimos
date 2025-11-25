using Deimos.AST.Declarations;
using Deimos.AST.Statements;
using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Private
{
    public abstract class AbstractFunctionDeclaration : MemberDeclaration
    {
        public string Name { get; }
        public ReadOnlyArray<TypeParameterNode> TypeParameters { get; }
        public ReadOnlyArray<ParameterNode> Parameters { get; }
        public TypeNode? ReturnType { get; }
        public BlockStatement Body { get; }

        public bool HasExplicitReturnType => ReturnType != null;

        public AbstractFunctionDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameters,
            IEnumerable<ParameterNode> parameters,
            TypeNode? returnType,
            BlockStatement body,
            TextRange range
        ) : base(modifiers, range)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameters = typeParameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameters));
            Parameters = parameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameters));
            ReturnType = returnType;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public AbstractFunctionDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameters,
            IEnumerable<ParameterNode> parameters,
            TypeNode? returnType,
            BlockStatement body,
            TextIndex from,
            TextIndex to
        ) : base(modifiers, from, to)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameters = typeParameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameters));
            Parameters = parameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameters));
            ReturnType = returnType;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public AbstractFunctionDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameters,
            IEnumerable<ParameterNode> parameters,
            TypeNode? returnType,
            BlockStatement body,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(modifiers, fromLine, fromColumn, toLine, toColumn)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameters = typeParameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameters));
            Parameters = parameters?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parameters));
            ReturnType = returnType;
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
        }

        public override string ToString(Indentation indent)
        {
            var returnTypeStr = ReturnType != null ? $": {ReturnType}" : string.Empty;
            var typeParametersStr = TypeParameters.Count > 0 ? $"<{string.Join(", ", TypeParameters)}>" : string.Empty;
            var parametersStr = Parameters.Count > 0 ? string.Join(", ", Parameters) : string.Empty;
            return $"func {Name}{typeParametersStr}({parametersStr}){returnTypeStr} {Body.ToString(indent)}";
        }
    }
}

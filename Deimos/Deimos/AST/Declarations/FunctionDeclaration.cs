#nullable enable

using Deimos.AST.Statements;
using Deimos.Lexer;
using System.Collections.Generic;

namespace Deimos.AST.Declarations
{
    public sealed class FunctionDeclaration : Private.AbstractFunctionDeclaration
    {
        public FunctionDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameters,
            IEnumerable<ParameterNode> parameters,
            TypeNode? returnType,
            BlockStatement body,
            TextRange range
        ) : base(name, modifiers, typeParameters, parameters, returnType, body, range) { }

        public FunctionDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameters,
            IEnumerable<ParameterNode> parameters,
            TypeNode? returnType,
            BlockStatement body,
            TextIndex from,
            TextIndex to
        ) : base(name, modifiers, typeParameters, parameters, returnType, body, from, to) { }

        public FunctionDeclaration(
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
        ) : base(name, modifiers, typeParameters, parameters, returnType, body, fromLine, fromColumn, toLine, toColumn) { }
    }
}

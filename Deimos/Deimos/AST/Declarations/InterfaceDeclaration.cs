#nullable enable

using Deimos.Lexer;
using System.Collections.Generic;

namespace Deimos.AST.Declarations
{
    public sealed class InterfaceDeclaration : Private.AbstractClassDeclaration
    {
        public InterfaceDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            TextRange range
        ) : base(name, modifiers, typeParameterNodes, parents, members, range) { }

        public InterfaceDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            TextIndex from,
            TextIndex to
        ) : base(name, modifiers, typeParameterNodes, parents, members, from, to) { }

        public InterfaceDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(name, modifiers, typeParameterNodes, parents, members, fromLine, fromColumn, toLine, toColumn) { }

        private protected sealed override string ClassDeclarationTypeName => "interface";
    }
}

#nullable enable

using Deimos.Lexer;
using System.Collections.Generic;

namespace Deimos.AST.Declarations
{
    public sealed class ClassDeclaration : Private.AbstractClassDeclaration
    {
        public ClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<MemberDeclaration> members,
            TextRange range
        ) : base(name, modifiers, typeParameterNodes, parents, members, range) { }

        public ClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<MemberDeclaration> members,
            TextIndex from,
            TextIndex to
        ) : base(name, modifiers, typeParameterNodes, parents, members, from, to) { }

        public ClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<MemberDeclaration> members,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(name, modifiers, typeParameterNodes, parents, members, fromLine, fromColumn, toLine, toColumn) { }

        private protected sealed override string ClassDeclarationTypeName => "class";
    }
}

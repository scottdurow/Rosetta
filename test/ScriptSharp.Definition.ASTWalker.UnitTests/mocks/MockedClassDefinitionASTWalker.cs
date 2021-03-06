﻿/// <summary>
/// MockedClassDefinitionASTWalker.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.ScriptSharp.Definition.AST.UnitTests.Mocks
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using Rosetta.ScriptSharp.Definition.Translation;

    /// <summary>
    /// Mock for <see cref="ClassDefinitionASTWalker"/>.
    /// </summary>
    public class MockedClassDefinitionASTWalker : ClassDefinitionASTWalker
    {
        protected MockedClassDefinitionASTWalker(ClassDefinitionASTWalker original, bool generateTranslationUniOnProtectedMembers)
            : base(original)
        {
            // Reassigning since base class operated on it
            this.classDeclaration = MockedClassDefinitionTranslationUnit.Create(this.classDeclaration as ClassDefinitionTranslationUnit);

            this.generateTranslationUniOnProtectedMembers = generateTranslationUniOnProtectedMembers;
        }

        public new static MockedClassDefinitionASTWalker Create(CSharpSyntaxNode node, bool generateTranslationUniOnProtectedMembers)
        {
            return new MockedClassDefinitionASTWalker(ClassDefinitionASTWalker.Create(node), generateTranslationUniOnProtectedMembers);
        }

        public MockedClassDefinitionTranslationUnit ClassDefinition
        {
            get { return this.classDeclaration as MockedClassDefinitionTranslationUnit; }
        }
    }
}

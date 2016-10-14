﻿/// <summary>
/// ConstructorDeclarationTranslationUnitFactory.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.AST.Factories
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;
    using Rosetta.AST.Helpers;

    /// <summary>
    /// Generic helper.
    /// </summary>
    public class ConstructorDeclarationTranslationUnitFactory : ITranslationUnitFactory
    {
        private readonly CSharpSyntaxNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="node"></param>
        public ConstructorDeclarationTranslationUnitFactory(CSharpSyntaxNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node), "A node must be specified!");
            }

            this.node = node;
        }

        /// <summary>
        /// Creates a <see cref="ConstructorDeclarationTranslationUnitFactory"/>.
        /// </summary>
        /// <returns>A <see cref="ConstructorDeclarationTranslationUnitFactory"/>.</returns>
        public ITranslationUnit Create()
        {
            ConstructorDeclaration helper = new ConstructorDeclaration(this.node as ConstructorDeclarationSyntax);

            var constructorDeclaration = this.CreateTranslationUnit(helper.Visibility) as MethodSignatureDeclarationTranslationUnit;

            foreach (Parameter parameter in helper.Parameters)
            {
                constructorDeclaration.AddArgument(ArgumentDefinitionTranslationUnit.Create(
                    TypeIdentifierTranslationUnit.Create(parameter.TypeName),
                    IdentifierTranslationUnit.Create(parameter.IdentifierName)));
            }

            return constructorDeclaration;
        }

        /// <summary>
        /// Creates the translation unit.
        /// </summary>
        /// <remarks>
        /// Must return a type inheriting from <see cref="MethodSignatureDeclarationTranslationUnit"/>.
        /// </remarks>
        /// <param name="visibility"></param>
        /// <returns></returns>
        protected virtual ITranslationUnit CreateTranslationUnit(VisibilityToken visibility)
        {
            return ConstructorDeclarationTranslationUnit.Create(visibility);
        }
    }
}

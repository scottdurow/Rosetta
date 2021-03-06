﻿/// <summary>
/// PropertyDeclarationTranslationUnitFactory.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.AST.Factories
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;
    using Rosetta.AST.Helpers;
    using Rosetta.AST.Utilities;

    /// <summary>
    /// Factory for <see cref="PropertyDeclarationTranslationUnit"/>.
    /// </summary>
    public class PropertyDeclarationTranslationUnitFactory : TranslationUnitFactory, ITranslationUnitFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel">The semantic model</param>
        public PropertyDeclarationTranslationUnitFactory(CSharpSyntaxNode node, SemanticModel semanticModel = null) 
            : base(node, semanticModel)
        {
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="PropertyDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public PropertyDeclarationTranslationUnitFactory(PropertyDeclarationTranslationUnitFactory other) 
            : base(other)
        {
        }

        /// <summary>
        /// Creates a <see cref="PropertyDeclarationTranslationUnitFactory"/>.
        /// </summary>
        /// <returns>A <see cref="PropertyDeclarationTranslationUnitFactory"/>.</returns>
        public ITranslationUnit Create()
        {
            if (this.DoNotCreateTranslationUnit)
            {
                return null;
            }

            PropertyDeclaration helper = this.CreateHelper(this.Node as PropertyDeclarationSyntax, this.SemanticModel);

            var propertyDeclaration = this.CreateTranslationUnit(
                helper.Modifiers,
                TypeIdentifierTranslationUnit.Create(helper.Type.FullName.MapType()),
                IdentifierTranslationUnit.Create(helper.Name),
                helper.HasGet,
                helper.HasSet);

            return propertyDeclaration;
        }

        /// <summary>
        /// Gets a value indicating whether the factory should return <code>null</code>.
        /// </summary>
        protected virtual bool DoNotCreateTranslationUnit
        {
            get { return false; }
        }

        /// <summary>
        /// Creates the translation unit.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="hasGet"></param>
        /// <param name="hasSet"></param>
        /// <returns></returns>
        protected virtual ITranslationUnit CreateTranslationUnit(
            ModifierTokens modifiers, ITranslationUnit type, ITranslationUnit name, bool hasGet, bool hasSet)
        {
            return PropertyDeclarationTranslationUnit.Create(modifiers, type, name, hasGet, hasSet);
        }

        /// <summary>
        /// Creates the helper.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// Returned type must be a derived type of <see cref="PropertyDeclaration"/>.
        /// </remarks>
        protected virtual PropertyDeclaration CreateHelper(PropertyDeclarationSyntax node, SemanticModel semanticModel)
        {
            return new PropertyDeclaration(node, semanticModel);
        }
    }
}

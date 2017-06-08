﻿/// <summary>
/// FieldDeclarationSyntaxFactory.cs
/// Andrea Tino - 2017
/// </summary>

namespace Rosetta.Reflection.Factories
{
    using System;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Reflection.Helpers;
    using Rosetta.Reflection.Proxies;

    /// <summary>
    /// Factory for generating a <see cref="FieldDeclarationSyntax"/>.
    /// </summary>
    public class FieldDeclarationSyntaxFactory : ISyntaxFactory
    {
        private readonly IFieldInfoProxy fieldInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeclarationSyntaxFactory"/> class.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="withBody"></param>
        public FieldDeclarationSyntaxFactory(IFieldInfoProxy fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            this.fieldInfo = fieldInfo;
        }

        /// <summary>
        /// Creates the <see cref="PropertyDeclarationSyntax"/>.
        /// </summary>
        /// <returns></returns>
        public SyntaxNode Create()
        {
            var varDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(this.fieldInfo.FieldType.FullName), 
                new SeparatedSyntaxList<VariableDeclaratorSyntax>()
                    .Add(SyntaxFactory.VariableDeclarator(this.fieldInfo.Name)));
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(varDeclaration);

            // Defining accessibility
            fieldDeclaration = fieldDeclaration.AddModifiers(SyntaxFactory.Token(new Visibility(this.fieldInfo).Token));

            return fieldDeclaration;
        }
    }
}

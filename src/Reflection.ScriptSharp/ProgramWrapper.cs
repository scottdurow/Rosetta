﻿/// <summary>
/// ProgramWrapper.cs
/// Andrea Tino - 2017
/// </summary>

namespace Rosetta.Reflection.ScriptSharp
{
    using System;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using Rosetta.AST;
    using Rosetta.Diagnostics.Logging;
    using Rosetta.Reflection.Proxies;
    using Rosetta.ScriptSharp.Definition.AST;

    /// <summary>
    /// Initiates the translation.
    /// </summary>
    public class ProgramWrapper : Rosetta.Reflection.ProgramWrapper
    {
        // TODO: Rename this whole assembly into Reflection.ScriptSharp.Definition

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramWrapper"/> class.
        /// </summary>
        /// <param name="assemblyPath"></param>
        public ProgramWrapper(string assemblyPath) 
            : base(assemblyPath)
        {
        }

        protected override IASTBuilder CreateASTBuilder(IAssemblyProxy assembly) => new ASTBuilder(assembly) { Logger = this.Logger };

        protected override IASTWalker CreateASTWalker(CSharpSyntaxNode node, SemanticModel semanticModel)
        {
            var walker = ProgramDefinitionASTWalker.Create(node, null, semanticModel);
            walker.Logger = this.Logger;

            return walker;
        }
    }
}

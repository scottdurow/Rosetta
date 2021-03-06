﻿/// <summary>
/// StatementASTWalker.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;

    /// <summary>
    /// Describes walkers in statements (statements that have blocks).
    /// TODO: Consider making it abstract
    /// </summary>
    public class StatementASTWalker : CSharpSyntaxWalker, IASTWalker
    {
        // TODO: Find common base with ASTWalker

        protected CSharpSyntaxNode node;
        protected SemanticModel semanticModel;
        
        /// <summary>
        /// Derives should fill this.
        /// </summary>
        protected StatementTranslationUnit statement;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementASTWalker"/> class.
        /// </summary>
        /// <param name="node"></param>
        protected StatementASTWalker(CSharpSyntaxNode node, SemanticModel semanticModel)
        {
            var statementSyntaxNode = node as StatementSyntax;
            if (statementSyntaxNode == null)
            {
                throw new ArgumentException(
                    string.Format("Specified node is not of type {0}",
                    typeof(StatementSyntax).Name));
            }

            this.node = node;
            this.semanticModel = semanticModel;
            this.statement = null;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="StatementASTWalker"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public StatementASTWalker(StatementASTWalker other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            this.node = other.node;
            this.semanticModel = other.semanticModel;
            this.statement = other.statement;
        }

        /// <summary>
        /// Walk the whole tree starting from specified <see cref="CSharpSyntaxNode"/> and 
        /// build the translation unit tree necessary for generating TypeScript output.
        /// </summary>
        /// <returns>The root of the translation unit tree.</returns>
        public ITranslationUnit Walk()
        {
            // Visiting
            this.Visit(node);

            this.WalkCore();

            // Returning root
            return this.statement;
        }

        /// <summary>
        /// TODO: Consider making it abstract
        /// </summary>
        protected virtual void WalkCore()
        {
        }

        #region CSharpSyntaxWalker overrides

        /// <summary>
        /// TODO: Remove
        /// </summary>
        /// <param name="node"></param>
        public override void VisitIfStatement(IfStatementSyntax node)
        {
            this.VisitIfStatementCore(node);
            this.InvokeIfStatementVisited(this, new WalkerEventArgs());

            if (this.ShouldWalkInto)
            {
                base.VisitIfStatement(node);
            }
        }

        // TODO: Remove
        protected virtual void VisitIfStatementCore(IfStatementSyntax node)
        {
        }

        #endregion

        #region Walk events

        /// <summary>
        /// Overrides should use this in order to instruct the class whether it is necessary to walk
        /// into every node by calling the base `Visit` method.
        /// TODO: Remove
        /// </summary>
        protected virtual bool ShouldWalkInto
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event WalkerEvent IfStatementVisited;

        #endregion

        private void InvokeIfStatementVisited(object sender, WalkerEventArgs e)
        {
            if (this.IfStatementVisited != null)
            {
                this.IfStatementVisited(sender, e);
            }
        }
    }
}

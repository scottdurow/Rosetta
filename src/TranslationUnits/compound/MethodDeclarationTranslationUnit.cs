﻿/// <summary>
/// MethodDeclarationTranslationUnit.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Class describing methods.
    /// </summary>
    public class MethodDeclarationTranslationUnit : MethodSignatureDeclarationTranslationUnit
    {
        // TODO: Use StatementsGroupTranslationUnit
        private IEnumerable<ITranslationUnit> statements;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclarationTranslationUnit"/> class.
        /// </summary>
        protected MethodDeclarationTranslationUnit() : base(IdentifierTranslationUnit.Empty, VisibilityToken.None)
        {
            this.statements = new List<ITranslationUnit>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="returnType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public new static MethodDeclarationTranslationUnit Create(VisibilityToken visibility, ITranslationUnit returnType, ITranslationUnit name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new MethodDeclarationTranslationUnit()
            {
                Visibility = visibility,
                Name = name,
                ReturnType = returnType
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<ITranslationUnit> InnerUnits
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Translate the unit into TypeScript.
        /// </summary>
        /// <returns></returns>
        public override string Translate()
        {
            FormatWriter writer = new FormatWriter()
            {
                Formatter = this.Formatter
            };

            // Opening declaration
            writer.WriteLine("{0} {1} {2}{3} {4}", 
                TokenUtility.ToString(this.Visibility), 
                this.ReturnType.Translate(), 
                this.Name.Translate(), 
                SyntaxUtility.ToBracketEnclosedList(this.Arguments.Select(unit => unit.Translate())),
                Lexems.OpenCurlyBracket);

            // Statements
            // The body, we render them as a list of semicolon/newline separated elements
            foreach (ITranslationUnit statement in this.statements)
            {
                writer.WriteLine("{0}{1}", 
                    statement.Translate(),
                    ShouldRenderSemicolon(statement) ? Lexems.Semicolon : string.Empty);
            }

            // Closing declaration
            writer.WriteLine("{0}", Lexems.CloseCurlyBracket);

            return writer.ToString();
        }

        #region Compound translation unit methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translationUnit"></param>
        public void AddStatement(ITranslationUnit translationUnit)
        {
            if (translationUnit == null)
            {
                throw new ArgumentNullException(nameof(translationUnit));
            }

            if (translationUnit as NestedElementTranslationUnit != null)
            {
                ((NestedElementTranslationUnit)translationUnit).NestingLevel = this.NestingLevel + 1;
            }

            ((List<ITranslationUnit>)this.statements).Add(translationUnit);
        }

        #endregion

        private static bool ShouldRenderSemicolon(ITranslationUnit statement)
        {
            var type = statement.GetType();

            var shouldNotRenderSemicolon = type == typeof(ConditionalStatementTranslationUnit);

            return !shouldNotRenderSemicolon;
        }
    }
}

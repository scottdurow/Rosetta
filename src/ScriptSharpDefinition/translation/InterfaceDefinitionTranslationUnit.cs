﻿/// <summary>
/// InterfaceDefinitionTranslationUnit.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.ScriptSharp.Definition.Translation
{
    using System;

    using Rosetta.Translation;

    /// <summary>
    /// Translation unit for describing interfaces for definitions.
    /// 
    /// TODO: Move to a separate project, this is specific to ScriptSharp.
    /// </summary>
    public class InterfaceDefinitionTranslationUnit : InterfaceDeclarationTranslationUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceDefinitionTranslationUnit"/> class.
        /// </summary>
        protected InterfaceDefinitionTranslationUnit() : base()
        {
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="InterfaceDefinitionTranslationUnit"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public InterfaceDefinitionTranslationUnit(InterfaceDefinitionTranslationUnit other)
            : base(other)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static new InterfaceDefinitionTranslationUnit Create(VisibilityToken visibility, ITranslationUnit name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Interface name cannot be null!");
            }

            return new InterfaceDefinitionTranslationUnit()
            {
                Visibility = visibility,
                Name = name
            };
        }
    }
}

﻿/// <summary>
/// MemberTranslationUnit.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Translation
{
    using System;

    /// <summary>
    /// Class describing scoped elements.
    /// </summary>
    public abstract class MemberTranslationUnit : ScopedElementTranslationUnit
    {
        /// <summary>
        /// The name of the member.
        /// </summary>
        protected ITranslationUnit Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTranslationUnit"/>.
        /// </summary>
        /// <param name="Name"></param>
        protected MemberTranslationUnit(ITranslationUnit name) 
            : this(name, ModifierTokens.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTranslationUnit"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="visibility"></param>
        protected MemberTranslationUnit(ITranslationUnit name, ModifierTokens modifiers)
            : base(modifiers)
        {
            this.Name = name;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="MemberTranslationUnit"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        protected MemberTranslationUnit(MemberTranslationUnit other) 
            : base(other)
        {
            this.Name = other.Name;
        }
    }
}

﻿/// <summary>
/// EnumClass.cs
/// Andrea Tino - 2017
/// </summary>

namespace Rosetta.Reflection.Helpers
{
    using System;

    using Rosetta.Reflection.Proxies;

    /// <summary>
    /// Helper for <see cref="ITypeInfoProxy"/> and <see cref="ITypeProxy"/> in order to get 
    /// information whether the type is <see cref="System.Enum"/>.
    /// </summary>
    public class EnumClass : NativeClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumClass"/> class.
        /// </summary>
        /// <param name="typeInfo">The <see cref="ITypeInfoProxy"/> to analyze.</param>
        public EnumClass(ITypeInfoProxy typeInfo)
            : base(typeInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumClass"/> class.
        /// </summary>
        /// <param name="type">The <see cref="ITypeProxy"/> to analyze.</param>
        public EnumClass(ITypeProxy type)
            : base(type)
        {
        }

        // TODO: Use reflection to get the full name
        protected override string ObjectClassFullName => "System.Enum";
    }
}

﻿/// <summary>
/// IAssemblyLoader.cs
/// Andrea Tino - 2017
/// </summary>

namespace Rosetta.Reflection
{
    using System;
    using System.IO;

    using Rosetta.Reflection.Proxies;

    /// <summary>
    /// Abstraction for loading the assembly.
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <returns>An <see cref="IAssemblyProxy"/> after loading it from a source.</returns>
        IAssemblyProxy Load();
    }
}

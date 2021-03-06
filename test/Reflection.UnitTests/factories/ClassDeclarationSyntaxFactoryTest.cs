﻿/// <summary>
/// ClassDeclarationSyntaxFactoryTest.cs
/// Andrea Tino - 2017
/// </summary>

namespace Rosetta.Reflection.Factories.UnitTests
{
    using System;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rosetta.Reflection.Factories;
    using Rosetta.Reflection.Proxies;
    using Rosetta.Reflection.UnitTests;

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ClassDeclarationSyntaxFactoryTest
    {
        [TestMethod]
        public void NameCorrectlyAcquired()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                class MyClass {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy classDefinition = assembly.LocateType("MyClass");

            Assert.IsNotNull(classDefinition);

            // Generating the AST
            var factory = new ClassDeclarationSyntaxFactory(classDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(ClassDeclarationSyntax), "Expected a class declaration node to be built");

            var classDeclarationSyntaxNode = syntaxNode as ClassDeclarationSyntax;

            var name = classDeclarationSyntaxNode.Identifier.Text;
            Assert.AreEqual("MyClass", name, "Class name not correctly acquired");
        }

        [TestMethod]
        public void BaseTypeNameCorrectlyAcquired()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                class MyBaseClass {
                }
                class MyClass : MyBaseClass {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy classDefinition = assembly.LocateType("MyClass");

            Assert.IsNotNull(classDefinition);

            // Generating the AST
            var factory = new ClassDeclarationSyntaxFactory(classDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(ClassDeclarationSyntax), "Expected a class declaration node to be built");

            var classDeclarationSyntaxNode = syntaxNode as ClassDeclarationSyntax;
            var baseList = classDeclarationSyntaxNode.BaseList.Types;

            Assert.AreEqual(1, baseList.Count, "Expected one base class only");

            var baseType = classDeclarationSyntaxNode.BaseList.Types.First();
            var baseTypeIdentifier = baseType.Type as IdentifierNameSyntax;

            Assert.IsNotNull(baseTypeIdentifier, "Identifier expected");

            var baseTypeName = baseTypeIdentifier.ToString();
            Assert.AreEqual("MyBaseClass", baseTypeName, "Base type full name not correct");
        }

        [TestMethod]
        public void InterfaceNamesCorrectlyAcquired()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                interface MyInterface1 {
                }
                interface MyInterface2 {
                }
                interface MyInterface3 {
                }
                class MyClass : MyInterface1, MyInterface2, MyInterface3 {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy classDefinition = assembly.LocateType("MyClass");

            Assert.IsNotNull(classDefinition);

            // Generating the AST
            var factory = new ClassDeclarationSyntaxFactory(classDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(ClassDeclarationSyntax), "Expected a class declaration node to be built");

            var classDeclarationSyntaxNode = syntaxNode as ClassDeclarationSyntax;
            var baseList = classDeclarationSyntaxNode.BaseList.Types;

            Assert.AreEqual(3, baseList.Count, "Expected 3 interfaces");

            Action<int, string> NameChecker = (index, expectedName) =>
            {
                var baseType = classDeclarationSyntaxNode.BaseList.Types.ElementAt(index);
                var baseTypeIdentifier = baseType.Type as IdentifierNameSyntax;

                Assert.IsNotNull(baseTypeIdentifier, "Identifier expected");

                var baseTypeName = baseTypeIdentifier.ToString();
                Assert.AreEqual(expectedName, baseTypeName, "Base type full name not correct");
            };

            NameChecker(0, "MyInterface1");
            NameChecker(1, "MyInterface2");
            NameChecker(2, "MyInterface3");
        }

        [TestMethod]
        public void ImplicitObjectClassInheritanceIsNotGenerated()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                class MyClass {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy classDefinition = assembly.LocateType("MyClass");

            Assert.IsNotNull(classDefinition);

            // Generating the AST
            var factory = new ClassDeclarationSyntaxFactory(classDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(ClassDeclarationSyntax), "Expected a class declaration node to be built");

            var classDeclarationSyntaxNode = syntaxNode as ClassDeclarationSyntax;

            var baseList = classDeclarationSyntaxNode.BaseList;
            Assert.IsNull(baseList, "No base class should have been generated");
        }

        [TestMethod]
        public void VisibilityCorrectlyAcquired()
        {
            // Public
            TestVisibility(@"
                namespace MyNamespace {
                    public class MyClass {
                    }
                }
            ", "MyClass", SyntaxKind.PublicKeyword);

            // Implicitely internal
            TestVisibility(@"
                namespace MyNamespace {
                    class MyClass {
                    }
                }
            ", "MyClass", null);
        }

        private static void TestVisibility(string source, string className, SyntaxKind? expectedVisibility)
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(source);

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy classDefinition = assembly.LocateType(className);

            Assert.IsNotNull(classDefinition);

            // Generating the AST
            var factory = new ClassDeclarationSyntaxFactory(classDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(ClassDeclarationSyntax), "Expected a class declaration node to be built");

            var classDeclarationSyntaxNode = syntaxNode as ClassDeclarationSyntax;

            var modifiers = classDeclarationSyntaxNode.Modifiers;

            if (expectedVisibility.HasValue)
            {
                Assert.IsTrue(Utils.CheckModifier(modifiers, expectedVisibility.Value), "Class does not have correct visibility");
                return;
            }

            Assert.AreEqual(0, modifiers.Count(), "Expected no modifier");
        }
    }
}

﻿/// <summary>
/// InterfaceDeclarationSyntaxFactoryTest.cs
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
    public class InterfaceDeclarationSyntaxFactoryTest
    {
        [TestMethod]
        public void NameCorrectlyAcquired()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                interface IMyInterface {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the interface
            ITypeInfoProxy interfaceDefinition = assembly.LocateType("IMyInterface");

            Assert.IsNotNull(interfaceDefinition);

            // Generating the AST
            var factory = new InterfaceDeclarationSyntaxFactory(interfaceDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(InterfaceDeclarationSyntax), "Expected an interface declaration node to be built");

            var interfaceDeclarationSyntaxNode = syntaxNode as InterfaceDeclarationSyntax;

            var name = interfaceDeclarationSyntaxNode.Identifier.Text;
            Assert.AreEqual("IMyInterface", name, "Interface name not correctly acquired");
        }

        [TestMethod]
        public void ExtendedInterfacesCorrectlyAcquired()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                interface MyExtendedInterface1 {
                }
                interface MyExtendedInterface2 {
                }
                interface MyExtendedInterface3 {
                }
                interface MyInterface : MyExtendedInterface1, MyExtendedInterface2, MyExtendedInterface3 {
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the class
            ITypeInfoProxy interfaceDefinition = assembly.LocateType("MyInterface");
            Assert.IsNotNull(interfaceDefinition);

            // Generating the AST
            var factory = new InterfaceDeclarationSyntaxFactory(interfaceDefinition);
            var syntaxNode = factory.Create() as InterfaceDeclarationSyntax;

            Assert.IsNotNull(syntaxNode, "An interface declaration node was expected to be built");

            var baseList = syntaxNode.BaseList.Types;
            Assert.AreEqual(3, baseList.Count, "Expected 3 extended interfaces");

            Action<int, string> ExtendedInterfaceChecker = (index, expectedName) =>
            {
                var baseType = baseList.ElementAt(index);
                var baseTypeIdentifier = baseType.Type as IdentifierNameSyntax;

                Assert.IsNotNull(baseTypeIdentifier, "Identifier expected");

                var baseTypeName = baseTypeIdentifier.ToString();
                Assert.AreEqual(expectedName, baseTypeName, "Base type full name not correct");
            };

            ExtendedInterfaceChecker(0, "MyExtendedInterface1");
            ExtendedInterfaceChecker(1, "MyExtendedInterface2");
            ExtendedInterfaceChecker(2, "MyExtendedInterface3");
        }

        [TestMethod]
        public void NoBody()
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(@"
                namespace MyNamespace {
                    public interface IMyInterface {
                        void MyMethod();
                    }
                }
            ");

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the interface
            ITypeInfoProxy interfaceDefinition = assembly.LocateType("IMyInterface");
            Assert.IsNotNull(interfaceDefinition);

            // Locating the method
            IMethodInfoProxy methodDeclaration = interfaceDefinition.LocateMethod("MyMethod");
            Assert.IsNotNull(methodDeclaration);

            // Generating the AST
            var factory = new MethodDeclarationSyntaxFactory(methodDeclaration, false);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(MethodDeclarationSyntax), "Expected a method declaration node to be built");

            var methodDeclarationSyntaxNode = syntaxNode as MethodDeclarationSyntax;

            var body = methodDeclarationSyntaxNode.Body;
            Assert.IsNull(body, "Expected a null body");
        }

        [TestMethod]
        public void VisibilityCorrectlyAcquired()
        {
            // Public
            TestVisibility(@"
                namespace MyNamespace {
                    public interface IMyInterface {
                    }
                }
            ", "IMyInterface", SyntaxKind.PublicKeyword);

            // Implicitely internal
            TestVisibility(@"
                namespace MyNamespace {
                    interface IMyInterface {
                    }
                }
            ", "IMyInterface", null);
        }

        private static void TestVisibility(string source, string interfaceName, SyntaxKind? expectedVisibility)
        {
            // Assembling some code
            IAssemblyLoader assemblyLoader = new Utils.AsmlDasmlAssemblyLoader(source);

            // Loading the assembly
            IAssemblyProxy assembly = assemblyLoader.Load();

            // Locating the interface
            ITypeInfoProxy interfaceDefinition = assembly.LocateType(interfaceName);

            Assert.IsNotNull(interfaceDefinition);

            // Generating the AST
            var factory = new InterfaceDeclarationSyntaxFactory(interfaceDefinition);
            var syntaxNode = factory.Create();

            Assert.IsNotNull(syntaxNode, "A node was expected to be built");
            Assert.IsInstanceOfType(syntaxNode, typeof(InterfaceDeclarationSyntax), "Expected an interface declaration node to be built");

            var interfaceDeclarationSyntaxNode = syntaxNode as InterfaceDeclarationSyntax;

            var modifiers = interfaceDeclarationSyntaxNode.Modifiers;

            if (expectedVisibility.HasValue)
            {
                Assert.IsTrue(Utils.CheckModifier(modifiers, expectedVisibility.Value), "Class does not have correct visibility");
                return;
            }

            Assert.AreEqual(0, modifiers.Count(), "Expected no modifier");
        }
    }
}

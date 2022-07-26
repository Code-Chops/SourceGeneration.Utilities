using CodeChops.SourceGeneration.Utilities.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public class TypeSymbolTests
{
	private static readonly SyntaxTree SyntaxTree = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeChops.Test;

private abstract partial record TestRecord<T>
{
}

public struct TestStruct
{
}

internal enum TestEnum
{
} 

public static class TestClass
{
}
");

	private static SemanticModel SemanticModel { get; } 
	
	static TypeSymbolTests()
	{
		var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
		var compilation = CSharpCompilation.Create(
			assemblyName: nameof(TypeSymbolTests),
			syntaxTrees: new[] { SyntaxTree }, 
			references: new[] { mscorlib });

		SemanticModel = compilation.GetSemanticModel(SyntaxTree);
	}
	
	[Fact]
	public void Get_FullTypeName_Test()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetFullTypeNameWithGenericParameters();
		
		Assert.Equal("global::CodeChops.Test.TestRecord<T>", fullTypeName);
	}
	
	[Fact]
	public void Get_FullTypeName_WithoutGenericParameters_Test()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetFullTypeNameWithoutGenericParameters();
		
		Assert.Equal("global::CodeChops.Test.TestRecord", fullTypeName);
	}

	
	[Fact]
	public void Get_TypeName_WithGenericParameters_Test()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetTypeNameWithGenericParameters();
		
		Assert.Equal("TestRecord<T>", fullTypeName);
	}

	[Theory]
	[InlineData(typeof(RecordDeclarationSyntax), 	"private abstract record class",			false)]
	[InlineData(typeof(StructDeclarationSyntax), 	"public struct",							false)]
	[InlineData(typeof(EnumDeclarationSyntax),	 	"internal enum",							false)]
	[InlineData(typeof(ClassDeclarationSyntax),		"public static class",						false)]
	[InlineData(typeof(RecordDeclarationSyntax), 	"private abstract partial record class",	true)]
	[InlineData(typeof(StructDeclarationSyntax), 	"public partial struct",					true)]
	[InlineData(typeof(EnumDeclarationSyntax),	 	"internal enum",							true)]
	[InlineData(typeof(ClassDeclarationSyntax),		"public static partial class",				true)]
	public void Get_ClassDefinition_Test(Type type, string expectedDefinition, bool makePartial)
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().Single(node => node.GetType() == type);
		var namedTypeSymbol = (ITypeSymbol)SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;
		
		var definition = namedTypeSymbol.GetObjectDefinition(makePartial);
		
		Assert.Equal(expectedDefinition, definition);
	}
}